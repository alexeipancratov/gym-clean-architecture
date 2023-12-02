using Ardalis.Result;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string SubscriptionType, Guid AdminId) : IRequest<Result<Subscription>>;
