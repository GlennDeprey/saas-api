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
            .Get<MediatROptions>() ?? new MediatROptions();

        services.AddMediatR(config =>
        {
            config.LicenseKey = mediatROptions.LicenseKey ?? string.Empty;
            config.RegisterServicesFromAssemblies(moduleAssemblies);
            config.AddOpenBehavior(typeof(Behaviors.RequestLoggingPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);

        return services;
    }
}
