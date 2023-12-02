using Ardalis.Result;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.GetSubscription
{
    public record GetSubscriptionCommand(Guid SubscriptionId) : IRequest<Result<Subscription>>;
}