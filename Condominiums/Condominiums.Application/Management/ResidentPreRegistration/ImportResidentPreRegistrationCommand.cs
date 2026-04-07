namespace Condominiums.Application.Management.ResidentPreRegistration;

public class ImportResidentPreRegistrationCommand
{
    public int CondominiumId { get; set; }
    public required string FileName { get; set; }
    public required byte[] Content { get; set; }
}
