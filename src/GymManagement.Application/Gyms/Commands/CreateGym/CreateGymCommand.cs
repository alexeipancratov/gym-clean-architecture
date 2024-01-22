using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Attributes;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

[Authorize(Roles = "Admin")]
public record CreateGymCommand(string Name, Guid SubscriptionId) : IRequest<Result<Gym, OperationError>>;