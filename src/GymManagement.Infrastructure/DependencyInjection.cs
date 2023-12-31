using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Admins;
using GymManagement.Infrastructure.Common.Persistence;
using GymManagement.Infrastructure.Gyms.Persistence;
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
        
        // Repositories
        services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
        services.AddScoped<IGymsRepository, GymsRepository>();
        services.AddScoped<IAdminsRepository, AdminsRepository>();

        return services;
    }
}