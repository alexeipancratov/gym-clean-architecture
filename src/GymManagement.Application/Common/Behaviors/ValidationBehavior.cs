using Ardalis.Result;
using FluentValidation;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

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
        
        // Works in runtime
        return (dynamic)Result.Invalid(
            validationResult.Errors.Select(e => new ValidationError
            {
                Identifier = e.PropertyName, ErrorMessage = e.ErrorMessage
            }).ToArray());
    }
}