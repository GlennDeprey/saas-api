using Saas.Modules.Events.Application.Categories.GetCategories;
using Saas.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Modules.Events.Application.Categories.GetCategory;
using Microsoft.Extensions.Caching.Memory;

namespace Saas.Modules.Events.Presentation.Categories;

internal static class GetCategories
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, IMemoryCache cache) =>
        {
            const string cacheKey = "categories";
            var categories = cache.Get<IReadOnlyCollection<CategoryResponse>>(cacheKey);
            if (categories is not null)
            {
                return Results.Ok(categories);
            }

            var result = await sender.Send(new GetCategoriesQuery());

            if (result.IsSuccess)
            {
                cache.Set(cacheKey, result.Value, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.CATEGORIES);
    }
}
