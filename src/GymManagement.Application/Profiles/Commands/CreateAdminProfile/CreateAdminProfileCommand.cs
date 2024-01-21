using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Profiles.Commands.CreateAdminProfile;

public record CreateAdminProfileCommand(Guid UserId) : IRequest<Result<Guid, OperationError>>;