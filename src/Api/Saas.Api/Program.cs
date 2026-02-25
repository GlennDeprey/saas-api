using Saas.Api.Common.OpenApi;
using Saas.Api.Extensions;
using Saas.Modules.Events.Infrastructure;
using SaasApi.ServiceDefaults;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);

    // Add OpenTelemetry sink for Aspire Dashboard
    var otlpEndpoint = context.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
    if (!string.IsNullOrWhiteSpace(otlpEndpoint))
    {
        loggerConfig.WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = otlpEndpoint;
            options.Protocol = OtlpProtocol.Grpc;
            options.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = context.Configuration["OTEL_SERVICE_NAME"] ?? "saas-api"
            };
        });
    }
});

builder.AddServiceDefaults();
builder.AddSeqEndpoint("seq");

builder.Configuration.AddModuleConfiguration(["events"]);

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarOpenApi();
    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.UseSerilogRequestLogging();

await app.RunAsync();