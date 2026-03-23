namespace Condolife.Api.ViewModels;

public record ResponseError(string Message, List<string>? Errors = null);