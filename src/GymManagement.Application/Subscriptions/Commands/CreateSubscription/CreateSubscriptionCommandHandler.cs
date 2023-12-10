using Ardalis.Result;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubscriptionCommand, Result<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new Subscription(
            subscriptionType: request.SubscriptionType,
            adminId: request.AdminId);

        await _subscriptionsRepository.AddSubscription(subscription, cancellationToken);

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        return subscription;
    }
}