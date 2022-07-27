using AmazonSimpleEmail.API.Application.Services.Implementations;
using AmazonSimpleEmail.API.Application.Services.Interface;
using AmazonSimpleEmail.API.Data;
using AmazonSimpleEmail.API.Data.Entities;
using AmazonSimpleEmail.API.Email;
using AmazonSimpleEmail.API.Utilis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AmazonSimpleEmail.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyConfiguration(this IServiceCollection services)
    {
        // Usando com banco de dados em memória
        services.AddDbContext<AuthDbContext>(options => options.UseInMemoryDatabase("AuthDB"));

        services.AddIdentity<UserModel, IdentityRole>(options =>
        {
            // Configurações de senha
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 2;
            options.Password.RequiredUniqueChars = 0;
        }).AddErrorDescriber<IdentityPortugues>()
          .AddEntityFrameworkStores<AuthDbContext>()
          .AddDefaultTokenProviders();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailComposerService, EmailComposerService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddTransient<UserInitializerService>();

        return services;
    }
}
