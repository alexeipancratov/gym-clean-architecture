using GymManagement.Api.Services;
using GymManagement.Application.Common.Interfaces;

namespace GymManagement.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // These conventions are being used by the TranslateResultToActionResultAttribute only.
        services.AddControllers(); 
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails(); // Adds required services for the UseExceptionHandler.
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        
        return services;
    }
}