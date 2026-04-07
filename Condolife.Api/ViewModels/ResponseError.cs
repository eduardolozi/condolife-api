namespace Condolife.Api.ViewModels;

public record ResponseError(
    string Message,
    List<ResponseValidationError>? ValidationErrors = null);
