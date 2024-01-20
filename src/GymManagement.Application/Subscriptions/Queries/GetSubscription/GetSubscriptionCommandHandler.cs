using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<GetSubscriptionCommand, Result<Subscription, OperationError>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;

    public async Task<Result<Subscription, OperationError>> Handle(GetSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId,
            cancellationToken);

        return subscription
               ?? Result.Failure<Subscription, OperationError>(OperationError.NotFound("Subscription not found"));
    }
}