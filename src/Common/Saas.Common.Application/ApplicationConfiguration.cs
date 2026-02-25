using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saas.Common.Application.Options;
using System.Reflection;

namespace Saas.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] moduleAssemblies)
    {
        var mediatROptions = configuration
            .GetSection(MediatROptions.SECTION_NAME)
            .Get<MediatROptions>() ?? throw new InvalidOperationException("MediatR license key not found.");

        services.AddMediatR(config =>
        {
            config.LicenseKey = mediatROptions.LicenseKey;
            config.RegisterServicesFromAssemblies(moduleAssemblies);

            config.AddOpenBehavior(typeof(Behaviors.ExceptionHandlingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(Behaviors.RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(Behaviors.ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);

        return services;
    }
}
