using CSharpFunctionalExtensions;
using GymManagement.Application.Authentication.Models;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<Result<AuthenticationResult, OperationError>>;