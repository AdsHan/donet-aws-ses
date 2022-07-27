using AmazonSimpleEmail.API.Application.Validators;
using AmazonSimpleEmail.API.Data;
using FluentValidation.AspNetCore;

namespace AmazonSimpleEmail.API.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<SignInAuthValidator>());

        services.AddEndpointsApiExplorer();

        return services;
    }

    public static WebApplication UseApiConfiguration(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var service = services.GetRequiredService<UserInitializerService>();
                service.Initialize();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocorreu um erro na inicialização");
            }
        }

        return app;
    }

}