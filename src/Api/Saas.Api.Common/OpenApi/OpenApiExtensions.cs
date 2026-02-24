using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace Saas.Api.Common.OpenApi;

public static class OpenApiExtensions
{
    public static void MapScalarOpenApi(this WebApplication application)
    {
        application.MapOpenApi();
        application.MapScalarApiReference();
    }
}
