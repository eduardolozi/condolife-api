namespace Condolife.Api.ViewModels;

public record ResponseValidationError(
    string? Field,
    string Message,
    int? Line = null);
