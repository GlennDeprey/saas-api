using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Saas.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        Assembly[] moduleAssemblies)
    {
        services.AddMediatR(config =>
        {
            //configuration.GetSection("Mediatr:Token").Value;
            config.LicenseKey = "";
            config.RegisterServicesFromAssemblies(moduleAssemblies);
        });

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);

        return services;
    }
}
