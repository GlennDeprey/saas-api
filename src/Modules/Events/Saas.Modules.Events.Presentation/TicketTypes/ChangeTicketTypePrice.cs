using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Modules.Events.Presentation.ApiResults;
using Saas.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

namespace Saas.Modules.Events.Presentation.TicketTypes;

internal static class ChangeTicketTypePrice
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("ticket-types/{id}/price", async (Guid id, Request request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateTicketTypePriceCommand(id, request.Price));

                return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
            })
            .WithTags(Tags.TICKET_TYPES);
    }

    internal sealed class Request
    {
        public decimal Price { get; init; }
    }
}
