using Saas.Modules.Events.Infrastructure;
using Saas.Common.Application;
using Saas.Common.Infrastructure;
using Saas.Modules.Users.Infrastructure;

namespace Saas.Api.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonLayer(configuration);
        services.AddModules(configuration);

        return services;
    }

    private static IServiceCollection AddCommonLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("saasdb") ??
            throw new InvalidOperationException("Connection string 'saasdb' not found.");

        var redisConnectionString = configuration.GetConnectionString("redis") ??
            throw new InvalidOperationException("Connection string 'redis' not found.");

        services.AddInfrastructure(databaseConnectionString, redisConnectionString);
        return services;
    }

    private static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication(configuration,
            [
                Modules.Events.Application.AssemblyReference.Assembly,
                Modules.Users.Application.AssemblyReference.Assembly,
            ]);

        services.AddEventsModule(configuration);
        services.AddUsersModule(configuration);

        return services;
    }
}
