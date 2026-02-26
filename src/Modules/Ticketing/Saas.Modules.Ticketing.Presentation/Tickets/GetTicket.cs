using Saas.Common.Presentation.Endpoints;
using Saas.Common.Presentation.ApiResults;
using Saas.Modules.Ticketing.Application.Tickets.GetTicket;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Saas.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicket: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tickets/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetTicketQuery(id));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.TICKETS);
    }
}
