using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task AddSubscription(Subscription subscription, CancellationToken cancellationToken = default);
    
    ValueTask<Subscription?> GetByIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken);
}