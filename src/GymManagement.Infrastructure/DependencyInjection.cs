using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Persistence;
using GymManagement.Infrastructure.Subscriptions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Persistence
        services.AddDbContext<GymManagementDbContext>(options =>
        {
            options.UseSqlite("Data Source=gym-mgmt.db");
        });
        // TODO: Test if registering implementation directly works too.
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<GymManagementDbContext>());
        services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();

        return services;
    }
}