using Ardalis.Result;
using GymManagement.Application.Gyms.Commands;
using GymManagement.Domain.Gyms;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            options.AddBehavior<IPipelineBehavior<CreateGymCommand, Result<Gym>>, CreateGymCommandBehavior>();
        });

        return services;
    }
}