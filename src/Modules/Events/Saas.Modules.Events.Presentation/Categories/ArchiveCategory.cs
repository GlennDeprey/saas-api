using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Categories.ArchiveCategory;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.Categories;

internal class ArchiveCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}/archive", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ArchiveCategoryCommand(id));

            return result.Match(() => Results.Ok(), Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.CATEGORIES);
    }
}
