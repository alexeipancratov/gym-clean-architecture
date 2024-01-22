using System.Reflection;
using GymManagement.Application.Common.Attributes;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserProvider currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    // TODO: Figure out how to add the constraint below.
    // where TResponse : Result
{
    private ICurrentUserProvider _currentUserProvider = currentUserProvider;
    
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizeAttributes.Count == 0)
        {
            return next();
        }
        
        var requiredPermissions = authorizeAttributes
            .SelectMany(a => a.Permissions?.Split(',') ?? [])
            .ToList();

        var currentUser = currentUserProvider.GetCurrentUser();
        
        if (requiredPermissions.Except(currentUser.Permissions).Any())
        {
            return (dynamic)OperationError.Unauthorized("User is not authorized to perform this action");
        }

        return next();
    }
}