using Ardalis.Result;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success(Guid.NewGuid()));
    }
}