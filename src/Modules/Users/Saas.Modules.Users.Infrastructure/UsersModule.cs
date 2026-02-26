using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Users.Application.Abstractions.Data;
using Saas.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saas.Modules.Users.Infrastructure.Database;
using Saas.Modules.Users.Infrastructure.Users;
using Saas.Common.Infrastructure.Interceptors;
using Saas.Modules.Users.PublicApi;
using Saas.Modules.Users.Infrastructure.PublicApi;

namespace Saas.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("saasdb") ??
            throw new InvalidOperationException("Connection string 'saasdb' not found.");

        services.AddDbContext<UsersDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.USERS))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IUsersApi, UsersApi>();
    }
}
