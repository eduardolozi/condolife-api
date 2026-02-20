using System.Text.Json.Serialization;

namespace Identity.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin,
    Doorman,
    Resident
}