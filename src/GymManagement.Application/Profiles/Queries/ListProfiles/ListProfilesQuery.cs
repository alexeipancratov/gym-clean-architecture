using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Profiles.Queries.ListProfiles;

public record ListProfilesQuery(Guid UserId) : IRequest<Result<ListProfilesResult, OperationError>>;