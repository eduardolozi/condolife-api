using Condominiums.Application.Abstractions;
using Condominiums.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Condominiums.Application.Management.ResidentPreRegistration;

public class ImportResidentPreRegistrationUseCase(
    IValidator<ImportResidentPreRegistrationCommand> commandValidator,
    IValidator<ResidentPreRegistrationRow> rowValidator,
    ICondominiumDbContext dbContext,
    ICsvFileService csvFileService)
{
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
            throw new ValidationException(failures);
        }

        var normalizedRows = rows.Select(row => new NormalizedResidentPreRegistrationRow(
            row.LineNumber,
            ResidentPreRegistrationNormalizer.NormalizeText(row.Nome),
            ResidentPreRegistrationNormalizer.NormalizeCpf(row.Cpf),
            ResidentPreRegistrationNormalizer.NormalizeText(row.Apartamento),
            ResidentPreRegistrationNormalizer.NormalizeOptionalText(row.Bloco)))
            .ToList();

        failures.AddRange(ValidateDuplicateRows(normalizedRows));

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
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
            throw new ValidationException(GetUniqueConstraintFailures(normalizedRows, postgresException.ConstraintName));
        }

        return new ImportResidentPreRegistrationResult(residentProspects.Count);
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

    private static List<ValidationFailure> GetUniqueConstraintFailures(
        List<NormalizedResidentPreRegistrationRow> rows,
        string? constraintName)
    {
        return constraintName switch
        {
            "UX_ResidentProspects_CondominiumId_Cpf" => rows
                .Select(row => CreateFailure(nameof(ResidentPreRegistrationRow.Cpf), "O CPF já está cadastrado para este condomínio.", row.LineNumber))
                .ToList(),
            "UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock" => rows
                .Where(row => row.Block is null)
                .Select(row => CreateFailure(nameof(ResidentPreRegistrationRow.Apartamento), "O apartamento já está cadastrado para este condomínio.", row.LineNumber))
                .ToList(),
            "UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock" => rows
                .Where(row => row.Block is not null)
                .Select(row => CreateFailure(nameof(ResidentPreRegistrationRow.Apartamento), "A combinação apartamento/bloco já está cadastrada para este condomínio.", row.LineNumber))
                .ToList(),
            _ => [new ValidationFailure(nameof(ImportResidentPreRegistrationCommand), "Não foi possível concluir a importação porque existem dados duplicados para este condomínio.")]
        };
    }

    private static ValidationFailure CreateFailure(string propertyName, string message, int lineNumber)
    {
        return new ValidationFailure(propertyName, message)
        {
            CustomState = lineNumber
        };
    }

    private sealed record NormalizedResidentPreRegistrationRow(
        int LineNumber,
        string Name,
        string Cpf,
        string Apartment,
        string? Block);

    private sealed record ExistingResidentProspect(
        string Cpf,
        string Apartment,
        string? Block);
}
