using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Infrastructure.Subscriptions.Persistence
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private static readonly List<Subscription> Subscriptions = new();

        public async Task AddSubscription(Subscription subscription)
        {
            Subscriptions.Add(subscription);

            await Task.CompletedTask;
        }

        public Task<Subscription?> GetSubscriptionAsync(Guid subscriptionId)
        {
            return Task.FromResult(Subscriptions.Find(s => s.Id == subscriptionId));
        }
    }
}