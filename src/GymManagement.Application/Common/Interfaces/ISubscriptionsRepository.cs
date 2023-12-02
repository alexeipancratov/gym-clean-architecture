using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task AddSubscription(Subscription subscription);
    
    ValueTask<Subscription?> GetSubscriptionAsync(Guid subscriptionId);
}