using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainerToGym;

public record AddTrainerToGymCommand(Guid SubscriptionId, Guid GymId, Guid TrainerId)
    : IRequest<UnitResult<OperationError>>;