using Saas.Api.Common.OpenApi;
using Saas.Api.Extensions;
using Saas.Common.Application;
using Saas.Common.Infrastructure;
using Saas.Modules.Events.Infrastructure;
using SaasApi.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddApplication([Saas.Modules.Events.Application.AssemblyReference.Assembly]);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database") ??
            throw new InvalidOperationException("Connection string 'Database' not found.");

builder.Services.AddInfrastructure(databaseConnectionString);

builder.Services.AddEventsModule(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarOpenApi();
    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.Run();