using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;

namespace GymManagement.Domain.Common.Interfaces;

public interface IPasswordHasher
{
    public Result<string, OperationError> HashPassword(string password);
    
    bool IsCorrectPassword(string password, string hash);
}