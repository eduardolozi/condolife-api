using Condominiums.Application.Management.ResidentPreRegistration;

namespace Condominiums.Application.Abstractions;

public interface ICsvFileService
{
    byte[] GenerateEmptyFileWithHeader(params string[] headers);
    List<ResidentPreRegistrationRow> ReadResidentPreRegistrationFile(string fileName, byte[] content);
}
