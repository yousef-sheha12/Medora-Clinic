using MedoraClinic.Application.Interfaces.IServices;
using MedoraClinic.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedoraClinic.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
