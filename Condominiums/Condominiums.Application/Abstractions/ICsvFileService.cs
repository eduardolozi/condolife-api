namespace Condominiums.Application.Abstractions;

public interface ICsvFileService
{
    byte[] GenerateEmptyFileWithHeader(params string[] headers);
}
