using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<GetSubscriptionCommand, Result<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;

    public async Task<Result<Subscription>> Handle(GetSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId,
            cancellationToken);

        return subscription is null
            ? Result<Subscription>.NotFound()
            : Result<Subscription>.Success(subscription);
    }
}