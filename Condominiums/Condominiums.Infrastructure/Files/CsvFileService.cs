using System.Globalization;
using System.Text;
using Condominiums.Application.Abstractions;
using Condominiums.Application.Management.ResidentPreRegistration;

namespace Condominiums.Infrastructure.Files;

public class CsvFileService : ICsvFileService
{
    private static readonly string[] ExpectedHeaders =
    [
        "nome",
        "cpf",
        "apartamento",
        "bloco"
    ];

    public byte[] GenerateEmptyFileWithHeader(params string[] headers)
    {
        var content = string.Join(';', headers) + Environment.NewLine;
        return Encoding.UTF8.GetBytes(content);
    }

    public List<ResidentPreRegistrationRow> ReadResidentPreRegistrationFile(string fileName, byte[] content)
    {
        var extension = Path.GetExtension(fileName);
        if (!extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) &&
            !extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("O arquivo deve ser do tipo CSV ou TXT.");
        }

        var text = Encoding.UTF8.GetString(content)
            .Trim()
            .TrimStart('\uFEFF');
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidDataException("O arquivo deve possuir conteúdo.");
        }

        var lines = text
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (lines.Length < 2)
        {
            throw new InvalidDataException("O arquivo deve conter cabeçalho e ao menos uma linha de dados.");
        }

        var delimiter = DetectDelimiter(lines[0]);
        var headerColumns = SplitLine(lines[0], delimiter)
            .Select(NormalizeHeader)
            .ToArray();

        if (headerColumns.Length < ExpectedHeaders.Length || !ExpectedHeaders.SequenceEqual(headerColumns.Take(ExpectedHeaders.Length)))
        {
            throw new InvalidDataException("O arquivo deve conter o cabeçalho Nome;Cpf;Apartamento;Bloco.");
        }

        var rows = new List<ResidentPreRegistrationRow>();

        for (var index = 1; index < lines.Length; index++)
        {
            var columns = SplitLine(lines[index], delimiter);
            while (columns.Count < ExpectedHeaders.Length)
            {
                columns.Add(string.Empty);
            }

            rows.Add(new ResidentPreRegistrationRow
            {
                LineNumber = index + 1,
                Nome = columns[0].Trim(),
                Cpf = columns[1].Trim(),
                Apartamento = columns[2].Trim(),
                Bloco = NormalizeOptional(columns[3])
            });
        }

        return rows;
    }

    private static char DetectDelimiter(string headerLine)
    {
        if (headerLine.Contains(';'))
        {
            return ';';
        }

        throw new InvalidDataException("Não foi possí­vel identificar o separador do arquivo.");
    }

    private static List<string> SplitLine(string line, char delimiter)
    {
        var values = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        for (var index = 0; index < line.Length; index++)
        {
            var character = line[index];

            if (character == '"')
            {
                if (inQuotes && index + 1 < line.Length && line[index + 1] == '"')
                {
                    current.Append('"');
                    index++;
                    continue;
                }

                inQuotes = !inQuotes;
                continue;
            }

            if (character == delimiter && !inQuotes)
            {
                values.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(character);
        }

        values.Add(current.ToString());
        return values;
    }

    private static string NormalizeHeader(string value)
    {
        var normalized = value.Trim().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(char.ToLowerInvariant(character));
            }
        }

        return builder.ToString();
    }

    private static string? NormalizeOptional(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
