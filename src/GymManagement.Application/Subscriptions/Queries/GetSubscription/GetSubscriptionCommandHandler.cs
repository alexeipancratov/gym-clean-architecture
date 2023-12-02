using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Subscriptions.Commands.GetSubscription;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionCommandHandler : IRequestHandler<GetSubscriptionCommand, Result<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public GetSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<Result<Subscription>> Handle(GetSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetSubscriptionAsync(request.SubscriptionId);

        return subscription is null
            ? Result<Subscription>.NotFound()
            : Result<Subscription>.Success(subscription);
    }
}