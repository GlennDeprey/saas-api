using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Saas.Modules.Events.Application.Abstractions.Clock;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Domain.Categories;
using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Domain.TicketTypes;
using Saas.Modules.Events.Infrastructure.Categories;
using Saas.Modules.Events.Infrastructure.Clock;
using Saas.Modules.Events.Infrastructure.Database;
using Saas.Modules.Events.Infrastructure.Events;
using Saas.Modules.Events.Infrastructure.TicketTypes;
using Saas.Modules.Events.Presentation.Categories;
using Saas.Modules.Events.Presentation.Events;
using Saas.Modules.Events.Presentation.TicketTypes;

namespace Saas.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        TicketTypeEndpoints.MapEndpoints(app);
        CategoryEndpoints.MapEndpoints(app);
        EventEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.LicenseKey = configuration.GetSection("Mediatr:Token").Value;
            config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
        });

        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddPersistance(configuration);
        services.AddInfrastructure();

        return services;
    }

    private static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("Database") ??
            throw new InvalidOperationException("Connection string 'Database' not found.");

        var npgDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.EVENTS))
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        // Repositories
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    private static void AddInfrastructure(this IServiceCollection services)
    {
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}
