using Saas.Api.Common.OpenApi;
using Saas.Api.Extensions;
using Saas.Api.Middleware;
using Saas.Common.Presentation.Endpoints;
using SaasApi.ServiceDefaults;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSeqEndpoint("seq");

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);

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

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Configuration.AddModuleConfiguration(["events"]);

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarOpenApi();
    app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

await app.RunAsync();