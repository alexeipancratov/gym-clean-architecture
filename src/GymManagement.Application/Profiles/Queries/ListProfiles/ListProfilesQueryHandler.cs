using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Profiles.Queries.ListProfiles;

public class ListProfilesQueryHandler(
    IUsersRepository usersRepository) : IRequestHandler<ListProfilesQuery, Result<ListProfilesResult, OperationError>>
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    
    public async Task<Result<ListProfilesResult, OperationError>> Handle(ListProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure<ListProfilesResult, OperationError>(OperationError.NotFound("User not found"));
        }

        return new ListProfilesResult(user.AdminId, user.ParticipantId, user.TrainerId);
    }
}