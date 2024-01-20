using CSharpFunctionalExtensions;
using GymManagement.Application.Authentication.Models;
using GymManagement.Core.ErrorHandling;
using MediatR;

namespace GymManagement.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<Result<AuthenticationResult, OperationError>>;