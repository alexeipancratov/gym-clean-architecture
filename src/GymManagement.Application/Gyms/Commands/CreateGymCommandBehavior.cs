using Ardalis.Result;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands;

public class CreateGymCommandBehavior : IPipelineBehavior<CreateGymCommand, Result<Gym>>
{
    public async Task<Result<Gym>> Handle(
        CreateGymCommand request, RequestHandlerDelegate<Result<Gym>> next, CancellationToken cancellationToken)
    {
        var validator = new CreateGymCommandValidator();
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return Result<Gym>.Invalid(
                validationResult.Errors.Select(e => new ValidationError
                {
                    Identifier = e.ErrorCode, ErrorMessage = e.ErrorMessage
                }).ToArray());
        }
        
        return await next();
    }
}