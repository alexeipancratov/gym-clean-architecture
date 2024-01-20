using CSharpFunctionalExtensions;
using GymManagement.Api.Shared;
using GymManagement.Application.Authentication;
using GymManagement.Application.Authentication.Commands.Register;
using GymManagement.Application.Authentication.Models;
using GymManagement.Application.Authentication.Queries.Login;
using GymManagement.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Authentication;

[Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
        var result = await _sender.Send(command);

        return result.Match(
            authResult => base.Ok(MapToAuthResponse(authResult)),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var result = await _sender.Send(query);

        if (result.IsFailure && result.Error == AuthenticationErrors.InvalidCredentials)
        {
            return Problem(
                detail: result.Error.Message,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return result.Match(
            authResult => Ok(MapToAuthResponse(authResult)),
            Problem);
    }

    private static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);
    }
}