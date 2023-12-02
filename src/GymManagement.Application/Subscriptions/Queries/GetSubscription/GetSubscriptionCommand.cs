using Ardalis.Result;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries.GetSubscription
{
    public record GetSubscriptionCommand(Guid SubscriptionId) : IRequest<Result<Subscription>>;
}