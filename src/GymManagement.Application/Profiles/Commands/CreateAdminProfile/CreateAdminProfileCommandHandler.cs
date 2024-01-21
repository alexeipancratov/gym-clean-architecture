using CSharpFunctionalExtensions;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Admins;
using MediatR;

namespace GymManagement.Application.Profiles.Commands.CreateAdminProfile;

public class CreateAdminProfileCommandHandler(
    IUsersRepository usersRepository,
    IAdminsRepository adminsRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider) : IRequestHandler<CreateAdminProfileCommand, Result<Guid, OperationError>>
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IAdminsRepository _adminsRepository = adminsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    
    public async Task<Result<Guid,OperationError>> Handle(CreateAdminProfileCommand command,
        CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();
        
        if (currentUser.Id != command.UserId)
        {
            return Result.Failure<Guid, OperationError>(
                OperationError.Forbidden("User is forbidden to perform this action"));
        }
        
        var user = await _usersRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result.Failure<Guid, OperationError>(OperationError.NotFound("User not found"));
        }

        var createAdminProfileResult = user.CreateAdminProfile();
        var admin = new Admin(userId: user.Id, id: createAdminProfileResult.Value);

        _usersRepository.Update(user);
        await _adminsRepository.AddAdminAsync(admin);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return createAdminProfileResult;
    }
}