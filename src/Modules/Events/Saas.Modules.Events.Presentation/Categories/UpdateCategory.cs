using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Categories.UpdateCategory;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.Categories;

internal class UpdateCategory: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}", async (Guid id, Request request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateCategoryCommand(id, request.Name));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithTags(Tags.CATEGORIES);
    }

    internal sealed class Request
    {
        public required string Name { get; init; }
    }
}
