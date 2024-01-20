using CSharpFunctionalExtensions;
using GymManagement.Application.Authentication.Models;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUsersRepository usersRepository)
    : IRequestHandler<LoginQuery, Result<AuthenticationResult, OperationError>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUsersRepository _usersRepository = usersRepository;
    
    public async Task<Result<AuthenticationResult, OperationError>> Handle(LoginQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email);

        return user is null || !user.IsCorrectPasswordHash(request.Password, _passwordHasher)
            ? AuthenticationErrors.InvalidCredentials
            : new AuthenticationResult(user, _jwtTokenGenerator.GenerateToken(user));
    }
}