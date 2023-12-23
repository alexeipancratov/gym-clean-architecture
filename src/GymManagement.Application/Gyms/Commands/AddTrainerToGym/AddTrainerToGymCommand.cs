using Ardalis.Result;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainerToGym;

public record AddTrainerToGymCommand(Guid SubscriptionId, Guid GymId, Guid TrainerId) : IRequest<Result>;