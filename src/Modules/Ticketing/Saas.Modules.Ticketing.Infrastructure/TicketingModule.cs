using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saas.Common.Infrastructure.Interceptors;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Ticketing.Application.Abstractions.Data;
using Saas.Modules.Ticketing.Application.Carts;
using Saas.Modules.Ticketing.Domain.Customers;
using Saas.Modules.Ticketing.Infrastructure.Customers;
using Saas.Modules.Ticketing.Infrastructure.Database;
using Saas.Modules.Ticketing.Infrastructure.PublicApi;
using Saas.Modules.Ticketing.PublicApi;

namespace Saas.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add endpoints for module
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        services.AddPersistance(configuration);

        return services;
    }

    private static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("saasdb") ??
            throw new InvalidOperationException("Connection string 'Database' not found.");

        services.AddDbContext<TicketingDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.TICKETING))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());

        services.AddSingleton<CartService>();

        services.AddScoped<ITicketingApi, TicketingApi>();

    }
}
