using GymManagement.Domain.Common;
using GymManagement.Infrastructure.Common.Constants;
using GymManagement.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Infrastructure.Common.Middleware;

/// <remarks>
/// This middleware wraps its logic in a transaction so that either all event handlers
/// succeed or none of them do.
/// </remarks>
public class EventualConsistencyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, GymManagementDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsConstants.HttpContextDomainEventsQueueItemKey, out var value)
                    && value is Queue<IDomainEvent> domainEventQueue)
                {
                    while (domainEventQueue!.TryDequeue(out IDomainEvent? domainEvent))
                    {
                        await publisher.Publish(domainEvent);
                    }

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                // TODO: Notify client that even though they got a successful response, the changes failed to be
                // persisted due to an unexpected error.
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}