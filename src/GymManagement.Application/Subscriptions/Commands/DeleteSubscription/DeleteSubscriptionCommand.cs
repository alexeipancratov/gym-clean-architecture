using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

public record DeleteSubscriptionCommand(Guid SubscriptionId) : IRequest<UnitResult<OperationError>>;