using GymManagement.Core.ErrorHandling;

namespace GymManagement.Application.Authentication;

public static class AuthenticationErrors
{
    public static readonly OperationError InvalidCredentials
        = OperationError.Invalid("Invalid credentials.", "authentication.invalid_credentials");
}