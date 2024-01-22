using System.Security.Claims;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    public CurrentUser GetCurrentUser()
    {
        var id = GetClaimValues("id")
            .Select(Guid.Parse)
            .First();
        var permissions = GetClaimValues("permissions");
        var roles = GetClaimValues(ClaimTypes.Role);
        
        return new CurrentUser(id, permissions, roles);
    }
    
    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();
        
        return _httpContextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == claimType)
            .Select(c => c.Value)
            .ToList();
    }
}