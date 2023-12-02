using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Persistence;

namespace GymManagement.Infrastructure.Subscriptions.Persistence
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public SubscriptionsRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSubscription(Subscription subscription)
        {
            await _dbContext.Subscriptions.AddAsync(subscription);
        }

        public ValueTask<Subscription?> GetSubscriptionAsync(Guid subscriptionId)
        {
            return _dbContext.Subscriptions.FindAsync(subscriptionId);
        }
    }
}