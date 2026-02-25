using Saas.Modules.Events.Infrastructure;
using Saas.Common.Application;
using Saas.Common.Infrastructure;

namespace Saas.Api.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModules(configuration);
        services.AddCommonLayer(configuration);
        return services;
    }

    private static IServiceCollection AddCommonLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("saasdb") ??
            throw new InvalidOperationException("Connection string 'saasdb' not found.");

        services.AddApplication(configuration,
            [
                Modules.Events.Application.AssemblyReference.Assembly
            ]);
        services.AddInfrastructure(databaseConnectionString);
        return services;
    }

    private static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventsModule(configuration);
        return services;
    }
}
