using Saas.Api.Common.OpenApi;
using Saas.Api.Extensions;
using Saas.Modules.Events.Infrastructure;
using SaasApi.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

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