using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;

namespace GymManagement.Domain.Common.Interfaces;

/// <summary>
/// Provides methods for hashing and verifying passwords.
/// </summary>
/// <remarks>It lives in the Domain Layer because it's being used by domain objects.</remarks>
public interface IPasswordHasher
{
    public Result<string, OperationError> HashPassword(string password);
    
    bool IsCorrectPassword(string password, string hash);
}