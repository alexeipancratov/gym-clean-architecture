using Ardalis.Result;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands;

public record CreateGymCommand(string Name, Guid SubscriptionId) : IRequest<Result<Gym>>;