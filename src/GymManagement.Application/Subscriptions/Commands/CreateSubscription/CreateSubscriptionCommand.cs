using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionType SubscriptionType, Guid AdminId)
    : IRequest<Result<Subscription, OperationError>>;
