using GymManagement.Application.Common.Models;

namespace GymManagement.Application.Common.Interfaces;

/// <summary>
/// Defines a contract for a service that provides the current user.
/// </summary>
/// <remarks>Must be implemented by the Presentation layer, because only it has knowledge about a user
/// which is interacting with it.</remarks>
public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}