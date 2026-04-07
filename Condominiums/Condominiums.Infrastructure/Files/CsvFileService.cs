using System.Text;
using Condominiums.Application.Abstractions;

namespace Condominiums.Infrastructure.Files;

public class CsvFileService : ICsvFileService
{
    public byte[] GenerateEmptyFileWithHeader(params string[] headers)
    {
        var content = string.Join(';', headers) + Environment.NewLine;
        return Encoding.UTF8.GetBytes(content);
    }
}
