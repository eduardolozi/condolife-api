namespace Condominiums.Application.Management.ResidentPreRegistration;

public record ImportResidentPreRegistrationResult(
    bool Success,
    string Message,
    List<ImportResidentPreRegistrationRowResult> Rows);

public record ImportResidentPreRegistrationRowResult(
    int Line,
    ImportResidentPreRegistrationRowData Data,
    List<ImportResidentPreRegistrationRowError> Errors);

public record ImportResidentPreRegistrationRowData(
    string Name,
    string Cpf,
    string Apartment,
    string? Block);

public record ImportResidentPreRegistrationRowError(
    string Field,
    string Message);
