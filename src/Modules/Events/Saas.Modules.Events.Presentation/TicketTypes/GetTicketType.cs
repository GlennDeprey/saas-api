using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.TicketTypes.GetTicketType;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.TicketTypes;

internal class GetTicketType: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetTicketTypeQuery(id));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.TICKET_TYPES);
    }
}
