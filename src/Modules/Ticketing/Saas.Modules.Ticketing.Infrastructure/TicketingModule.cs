using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Ticketing.Application.Carts;

namespace Saas.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add endpoints for module
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        services.AddSingleton<CartService>();

        services.AddPersistance(configuration);

        return services;
    }

    private static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("saasdb") ??
            throw new InvalidOperationException("Connection string 'Database' not found.");

        //services.AddDbContext<EventsDbContext>((sp, options) =>
        //    options.UseNpgsql(
        //        databaseConnectionString,
        //        npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.EVENTS))
        //    .UseSnakeCaseNamingConvention()
        //    .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        //services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

    }
}
