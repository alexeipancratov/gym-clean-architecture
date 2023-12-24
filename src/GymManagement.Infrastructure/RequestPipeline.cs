using GymManagement.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GymManagement.Infrastructure;

public static class RequestPipeline
{
    /// <summary>
    /// Registers the Infrastructure layer ASP.NET Core middleware.
    /// </summary>
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();

        return builder;
    }
}