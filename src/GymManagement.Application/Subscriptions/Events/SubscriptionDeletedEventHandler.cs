using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Events;

public class SubscriptionDeletedEventHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IUnitOfWork unitOfWork)
    : INotificationHandler<SubscriptionDeletedEvent>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        Subscription? subscription =
            await _subscriptionsRepository.GetByIdAsync(notification.SubscriptionId, cancellationToken);

        if (subscription == null)
        {
            // TODO: Add resilient error handling, which would notify a user post-factum that subscription
            // failed to be deleted in the end.
            throw new InvalidOperationException("Subscription not found");
        }
        
        _subscriptionsRepository.Delete(subscription);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
    }
}