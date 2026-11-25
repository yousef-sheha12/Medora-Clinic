using MedoraClinic.Application.Interfaces.IRepos;
using MedoraClinic.Application.Interfaces.IServices;
using MedoraClinic.Infrastructure.Data;
using MedoraClinic.Infrastructure.Repositories;
using MedoraClinic.Infrastructure.Services;
using MedoraClinic.Infrastructure.Services.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedoraClinic.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found. Check ConnectionStrings__DefaultConnection in .env or environment variables.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


        services.AddOptions<EmailOptions>().Bind(configuration.GetSection(EmailOptions.SectionName));

        services.AddOptions<JwtOptions>().Bind(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
