using FluentValidation;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

/// <summary>
/// This behavior is responsible for validating MediatR requests data.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Not all requests have validators.
        if (_validator is null)
        {
            return await next();
        }
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid)
        {
            return await next();
        }
        
        // In runtime it will be implicitly casted to either Result or OperationError (both generic).
        // Currently, we don't support non-generic Result, because it works with string errors only.
        return (dynamic)OperationError.Invalid("Invalid fields", validationResult.Errors
            .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
            .ToArray());
    }
}