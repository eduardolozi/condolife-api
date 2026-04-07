using System.Text.RegularExpressions;

namespace Condominiums.Application.Management.ResidentPreRegistration;

public static partial class ResidentPreRegistrationNormalizer
{
    private static readonly Regex AllowedCpfFormatRegex = AllowedCpfFormat();

    public static string NormalizeCpf(string cpf)
    {
        return new string(cpf.Where(char.IsDigit).ToArray());
    }

    public static bool HasAllowedCpfFormat(string cpf)
    {
        return AllowedCpfFormatRegex.IsMatch(cpf.Trim());
    }

    public static string NormalizeText(string value)
    {
        return value.Trim();
    }

    public static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    [GeneratedRegex(@"^(?:\d{3}\.\d{3}\.\d{3}-\d{2}|\d{9}-\d{2}|\d{11})$")]
    private static partial Regex AllowedCpfFormat();
}
