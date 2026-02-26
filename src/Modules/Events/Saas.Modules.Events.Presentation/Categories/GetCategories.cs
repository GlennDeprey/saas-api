using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Hybrid;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Categories.GetCategories;

namespace Saas.Modules.Events.Presentation.Categories;

internal class GetCategories: IEndpoint
{
    private static readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(10)
    };

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, HybridCache cache) =>
        {
            const string cacheKey = "categories";

            var categories = await cache.GetOrCreateAsync(
                cacheKey,
                async _ =>
                {
                    var result = await sender.Send(new GetCategoriesQuery(), _);
                    return result.IsSuccess ? result.Value : [];
                },
                _cacheOptions);

            return Results.Ok(categories);
        })
        .WithTags(Tags.CATEGORIES);
    }
}
