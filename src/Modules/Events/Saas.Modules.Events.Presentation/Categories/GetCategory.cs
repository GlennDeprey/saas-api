using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Categories.GetCategory;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.Categories;

internal class GetCategory: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryQuery(id));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CATEGORIES);
    }
}
