using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Subscriptions.Persistence
{
    public class SubscriptionsRepository(GymManagementDbContext dbContext) : ISubscriptionsRepository
    {
        private readonly GymManagementDbContext _dbContext = dbContext;

        public async Task AddSubscription(Subscription subscription, CancellationToken cancellationToken = default)
        {
            await _dbContext.Subscriptions.AddAsync(subscription, cancellationToken);
        }

        public ValueTask<Subscription?> GetByIdAsync(Guid subscriptionId,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Subscriptions.FindAsync(new object?[] { subscriptionId },
                cancellationToken: cancellationToken);
        }

        public Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken)
        {
            _dbContext.Subscriptions.Update(subscription);
            
            return Task.CompletedTask;
        }
        
        public void Delete(Subscription subscription)
        {
            _dbContext.Subscriptions.Remove(subscription);
        }
    }
}