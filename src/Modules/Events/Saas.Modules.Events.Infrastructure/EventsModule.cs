using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saas.Common.Infrastructure.Interceptors;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Domain.Categories;
using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Domain.TicketTypes;
using Saas.Modules.Events.Infrastructure.Categories;
using Saas.Modules.Events.Infrastructure.Database;
using Saas.Modules.Events.Infrastructure.Events;
using Saas.Modules.Events.Infrastructure.TicketTypes;

namespace Saas.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
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

        services.AddDbContext<EventsDbContext>((sp, options) =>
            options.UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.EVENTS))
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        // Repositories
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}
