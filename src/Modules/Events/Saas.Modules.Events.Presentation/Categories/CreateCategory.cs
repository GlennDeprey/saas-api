using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Categories.CreateCategory;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.Categories;

internal  class CreateCategory: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(new CreateCategoryCommand(request.Name));

            return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.CATEGORIES);
    }

    internal sealed class Request
    {
        public required string Name { get; init; }
    }
}
