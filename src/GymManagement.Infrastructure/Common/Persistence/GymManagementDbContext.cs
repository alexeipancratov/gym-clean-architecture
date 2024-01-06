using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext(
    DbContextOptions options,
    IHttpContextAccessor httpContextAccessor,
    IPublisher publisher) : DbContext(options), IUnitOfWork
{
    /// <remarks>
    /// IHttpContextAccessor is a singleton, while HttpContext is scoped.
    /// Therefore, IHttpContextAccessor can be misused, so a safer approach is to
    /// create a middleware that would take the current HttpContext and store it in
    /// in an object that is scoped. Then, we can inject that object into the DbContext.
    /// </remarks>
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    private readonly IPublisher _publisher = publisher;
    
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    
    public DbSet<Gym> Gyms => Set<Gym>();
    
    public DbSet<Admin> Admins => Set<Admin>();

    public async Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        // get all domain events
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(events => events)
            .ToList();

        // We don't want the user to wait for the domain events to be published
        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            // E.g., there won't be a user waiting online if we're running tests.
            await PublishEvents(domainEvents, cancellationToken);
        }

        return await SaveChangesAsync(cancellationToken);
    }

    private async Task PublishEvents(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // We may already have a queue of domain events for this request if
        // another CommitChangesAsync() call was made within the same request.
        var domainEventQueue =
            _httpContextAccessor.HttpContext!.Items
                .TryGetValue(DomainEventsConstants.HttpContextDomainEventsQueueItemKey, out var queue)
                    && queue is Queue<IDomainEvent> existingQueue
                ? existingQueue
                : new Queue<IDomainEvent>();
        
        domainEvents.ForEach(domainEventQueue.Enqueue);
        
        _httpContextAccessor.HttpContext!
            .Items[DomainEventsConstants.HttpContextDomainEventsQueueItemKey] = domainEventQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}