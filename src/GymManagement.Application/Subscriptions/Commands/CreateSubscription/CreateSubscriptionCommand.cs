using Ardalis.Result;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string SubscriptionType, Guid AdminId) : IRequest<Result<Guid>>;
