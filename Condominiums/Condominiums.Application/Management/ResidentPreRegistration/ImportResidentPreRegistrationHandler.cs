using Condominiums.Application.Abstractions;
using Condominiums.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Condominiums.Application.Management.ResidentPreRegistration;

public class ImportResidentPreRegistrationHandler(
    IValidator<ImportResidentPreRegistrationCommand> commandValidator,
    IValidator<ResidentPreRegistrationRow> rowValidator,
    ICondominiumDbContext dbContext,
    ICsvFileService csvFileService)
{
    private const string SuccessMessage = "Arquivo processado com sucesso.";
    private const string InconsistencyMessage = "Arquivo processado com inconsistências.";

    public async Task<ImportResidentPreRegistrationResult> HandleAsync(ImportResidentPreRegistrationCommand command, CancellationToken ct)
    {
        await commandValidator.ValidateAndThrowAsync(command, ct);

        var condominiumExists = await dbContext.Condominiums
            .AnyAsync(x => x.Id == command.CondominiumId, ct);

        if (!condominiumExists)
        {
            throw new ApplicationException("O condomínio informado não foi encontrado.");
        }

        List<ResidentPreRegistrationRow> rows;
        try
        {
            rows = csvFileService.ReadResidentPreRegistrationFile(command.FileName, command.Content);
        }
        catch (InvalidDataException ex)
        {
            throw new ValidationException(
                [
                    new ValidationFailure(nameof(command.FileName), ex.Message)
                ]);
        }

        var normalizedRows = rows.Select(row => new NormalizedResidentPreRegistrationRow(
            row.LineNumber,
            ResidentPreRegistrationNormalizer.NormalizeText(row.Nome),
            ResidentPreRegistrationNormalizer.NormalizeCpf(row.Cpf),
            ResidentPreRegistrationNormalizer.NormalizeText(row.Apartamento),
            ResidentPreRegistrationNormalizer.NormalizeOptionalText(row.Bloco)))
            .ToList();

        var failures = new List<ValidationFailure>();

        foreach (var row in rows)
        {
            var validationResult = await rowValidator.ValidateAsync(row, ct);
            if (validationResult.IsValid)
            {
                continue;
            }

            failures.AddRange(validationResult.Errors.Select(error => new ValidationFailure(error.PropertyName, error.ErrorMessage)
            {
                AttemptedValue = error.AttemptedValue,
                CustomState = row.LineNumber
            }));
        }

        if (failures.Count > 0)
        {
            return BuildFailureResult(normalizedRows, failures);
        }

        failures.AddRange(ValidateDuplicateRows(normalizedRows));

        if (failures.Count > 0)
        {
            return BuildFailureResult(normalizedRows, failures);
        }

        var residentProspects = normalizedRows.Select(row => new ResidentProspect
        {
            CondominiumId = command.CondominiumId,
            Name = row.Name,
            Cpf = row.Cpf,
            Apartment = row.Apartment,
            Block = row.Block
        }).ToList();

        try
        {
            await dbContext.ResidentProspects.AddRangeAsync(residentProspects, ct);
            await dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresException && postgresException.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            var databaseFailures = await GetUniqueConstraintFailuresAsync(
                command.CondominiumId,
                normalizedRows,
                postgresException.ConstraintName,
                ct);

            return BuildFailureResult(normalizedRows, databaseFailures);
        }

        return new ImportResidentPreRegistrationResult(
            true,
            SuccessMessage,
            normalizedRows.Select(CreateRowResult).ToList());
    }

    private static List<ValidationFailure> ValidateDuplicateRows(List<NormalizedResidentPreRegistrationRow> rows)
    {
        var failures = new List<ValidationFailure>();

        foreach (var duplicatedCpfGroup in rows.GroupBy(x => x.Cpf).Where(group => group.Count() > 1))
        {
            failures.AddRange(duplicatedCpfGroup.Select(row =>
                CreateFailure(nameof(ResidentPreRegistrationRow.Cpf), "O CPF já foi informado em outra linha do arquivo para este condomínio.", row.LineNumber)));
        }

        foreach (var duplicatedApartmentGroup in rows
                     .Where(x => x.Block is null)
                     .GroupBy(x => x.Apartment)
                     .Where(group => group.Count() > 1))
        {
            failures.AddRange(duplicatedApartmentGroup.Select(row =>
                CreateFailure(nameof(ResidentPreRegistrationRow.Apartamento), "O apartamento já foi informado em outra linha do arquivo para este condomínio.", row.LineNumber)));
        }

        foreach (var duplicatedApartmentBlockGroup in rows
                     .Where(x => x.Block is not null)
                     .GroupBy(x => new { x.Apartment, x.Block })
                     .Where(group => group.Count() > 1))
        {
            failures.AddRange(duplicatedApartmentBlockGroup.Select(row =>
                CreateFailure(nameof(ResidentPreRegistrationRow.Apartamento), "A combinação apartamento/bloco já foi informada em outra linha do arquivo para este condomínio.", row.LineNumber)));
        }

        return failures;
    }

    private async Task<List<ValidationFailure>> GetUniqueConstraintFailuresAsync(
        int condominiumId,
        List<NormalizedResidentPreRegistrationRow> rows,
        string? constraintName,
        CancellationToken ct)
    {
        return constraintName switch
        {
            "UX_ResidentProspects_CondominiumId_Cpf" => await GetCpfConstraintFailuresAsync(condominiumId, rows, ct),
            "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock" => await GetApartmentWithoutBlockConstraintFailuresAsync(condominiumId, rows, ct),
            "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock" => await GetApartmentWithBlockConstraintFailuresAsync(condominiumId, rows, ct),
            _ => [new ValidationFailure(nameof(ImportResidentPreRegistrationCommand), "Não foi possível concluir a importação porque existem dados duplicados para este condomínio.")]
        };
    }

    private async Task<List<ValidationFailure>> GetCpfConstraintFailuresAsync(
        int condominiumId,
        List<NormalizedResidentPreRegistrationRow> rows,
        CancellationToken ct)
    {
        var cpfs = rows
            .Select(row => row.Cpf)
            .Distinct()
            .ToList();

        var existingCpfs = await dbContext.ResidentProspects
            .Where(x => x.CondominiumId == condominiumId && cpfs.Contains(x.Cpf))
            .Select(x => x.Cpf)
            .ToListAsync(ct);

        var existingCpfSet = existingCpfs.ToHashSet(StringComparer.Ordinal);

        return rows
            .Where(row => existingCpfSet.Contains(row.Cpf))
            .Select(row => CreateFailure(nameof(ResidentPreRegistrationRow.Cpf), "O CPF já está cadastrado para este condomínio.", row.LineNumber))
            .ToList();
    }

    private async Task<List<ValidationFailure>> GetApartmentWithoutBlockConstraintFailuresAsync(
        int condominiumId,
        List<NormalizedResidentPreRegistrationRow> rows,
        CancellationToken ct)
    {
        var apartments = rows
            .Where(row => row.Block is null)
            .Select(row => row.Apartment)
            .Distinct()
            .ToList();

        var existingApartments = await dbContext.ResidentProspects
            .Where(x => x.CondominiumId == condominiumId && x.Block == null && apartments.Contains(x.Apartment))
            .Select(x => x.Apartment)
            .ToListAsync(ct);

        var existingApartmentSet = existingApartments.ToHashSet(StringComparer.Ordinal);

        return rows
            .Where(row => row.Block is null && existingApartmentSet.Contains(row.Apartment))
            .Select(row => CreateFailure(nameof(ResidentPreRegistrationRow.Apartamento), "O apartamento já está cadastrado para este condomínio.", row.LineNumber))
            .ToList();
    }

    private async Task<List<ValidationFailure>> GetApartmentWithBlockConstraintFailuresAsync(
        int condominiumId,
        List<NormalizedResidentPreRegistrationRow> rows,
        CancellationToken ct)
    {
        var apartmentBlocks = rows
            .Where(row => row.Block is not null)
            .Select(row => new { row.Apartment, row.Block })
            .Distinct()
            .ToList();

        var existingApartmentBlocks = await dbContext.ResidentProspects
            .Where(x => x.CondominiumId == condominiumId && x.Block != null)
            .Select(x => new { x.Apartment, x.Block })
            .ToListAsync(ct);

        var existingApartmentBlockSet = existingApartmentBlocks
            .Where(x => apartmentBlocks.Any(key => key.Apartment == x.Apartment && key.Block == x.Block))
            .Select(x => $"{x.Apartment}|{x.Block}")
            .ToHashSet(StringComparer.Ordinal);

        return rows
            .Where(row => row.Block is not null && existingApartmentBlockSet.Contains($"{row.Apartment}|{row.Block}"))
            .Select(row => CreateFailure(nameof(ResidentPreRegistrationRow.Apartamento), "A combinação apartamento/bloco já está cadastrada para este condomínio.", row.LineNumber))
            .ToList();
    }

    private static ValidationFailure CreateFailure(string propertyName, string message, int lineNumber)
    {
        return new ValidationFailure(propertyName, message)
        {
            CustomState = lineNumber
        };
    }

    private static ImportResidentPreRegistrationResult BuildFailureResult(
        List<NormalizedResidentPreRegistrationRow> rows,
        List<ValidationFailure> failures)
    {
        var errorsByLine = failures
            .Where(failure => failure.CustomState is int)
            .GroupBy(failure => (int)failure.CustomState!)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => new ImportResidentPreRegistrationRowError(
                        MapField(failure.PropertyName),
                        failure.ErrorMessage))
                    .ToList());

        var results = rows
            .Select(row => CreateRowResult(
                row,
                errorsByLine.TryGetValue(row.LineNumber, out var rowErrors)
                    ? rowErrors
                    : []))
            .ToList();

        return new ImportResidentPreRegistrationResult(false, InconsistencyMessage, results);
    }

    private static ImportResidentPreRegistrationRowResult CreateRowResult(NormalizedResidentPreRegistrationRow row)
    {
        return CreateRowResult(row, []);
    }

    private static ImportResidentPreRegistrationRowResult CreateRowResult(
        NormalizedResidentPreRegistrationRow row,
        List<ImportResidentPreRegistrationRowError> errors)
    {
        return new ImportResidentPreRegistrationRowResult(
            row.LineNumber,
            new ImportResidentPreRegistrationRowData(
                row.Name,
                row.Cpf,
                row.Apartment,
                row.Block),
            errors);
    }

    private static string MapField(string propertyName)
    {
        return propertyName switch
        {
            nameof(ResidentPreRegistrationRow.Nome) => "name",
            nameof(ResidentPreRegistrationRow.Cpf) => "cpf",
            nameof(ResidentPreRegistrationRow.Apartamento) => "apartment",
            nameof(ResidentPreRegistrationRow.Bloco) => "block",
            _ => propertyName.ToLowerInvariant()
        };
    }

    private sealed record NormalizedResidentPreRegistrationRow(
        int LineNumber,
        string Name,
        string Cpf,
        string Apartment,
        string? Block);
}
