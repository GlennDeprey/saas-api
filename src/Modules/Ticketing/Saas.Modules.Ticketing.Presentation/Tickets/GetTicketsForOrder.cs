using Saas.Common.Presentation.Endpoints;
using Saas.Common.Presentation.ApiResults;
using Saas.Modules.Ticketing.Application.Tickets.GetTicketForOrder;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Saas.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicketsForOrder: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tickets/order/{orderId}", async (Guid orderId, ISender sender) =>
        {
            var result = await sender.Send(
                new GetTicketsForOrderQuery(orderId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.TICKETS);
    }
}
