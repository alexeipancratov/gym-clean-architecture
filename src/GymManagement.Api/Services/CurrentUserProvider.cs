using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();
        
        var user = _httpContextAccessor.HttpContext.User;
        var idClaim = user.Claims
            .First(c => c.Type == "id");
        var permissionClaims = user.Claims
            .Where(c => c.Type == "permissions")
            .SelectMany(c => c.Value.Split(','))
            .ToList();
        
        return new CurrentUser(Guid.Parse(idClaim.Value), permissionClaims);
    }
}