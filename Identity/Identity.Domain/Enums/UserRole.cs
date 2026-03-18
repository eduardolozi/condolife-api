using System.Text.Json.Serialization;

namespace Identity.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    CondomininiumAdministrator,
    Syndic,
    SubSyndic,
    FiscalCouncil,
    Doorman,
    Resident,
    Dependent
}