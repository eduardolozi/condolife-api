namespace Condominiums.Application.Management.ResidentPreRegistration;

public class ResidentPreRegistrationRow
{
    public int LineNumber { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public string Apartamento { get; init; } = string.Empty;
    public string? Bloco { get; init; }
}
