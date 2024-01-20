using CSharpFunctionalExtensions;
using GymManagement.Application.Authentication.Models;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Common.Interfaces;
using GymManagement.Domain.Users;
using MediatR;

namespace GymManagement.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUsersRepository usersRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterCommand, Result<AuthenticationResult, OperationError>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result<AuthenticationResult, OperationError>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await _usersRepository.ExistsByEmailAsync(request.Email))
        {
            return Result.Failure<AuthenticationResult, OperationError>(OperationError.Conflict("User already exists"));
        }

        var hashPasswordResult = _passwordHasher.HashPassword(request.Password);

        if (hashPasswordResult.IsFailure)
        {
            return hashPasswordResult.Error;
        }

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            hashPasswordResult.Value);

        await _usersRepository.AddUserAsync(user);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}