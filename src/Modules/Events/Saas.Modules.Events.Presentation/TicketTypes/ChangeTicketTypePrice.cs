using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.TicketTypes;

internal class ChangeTicketTypePrice: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("ticket-types/{id}/price", async (Guid id, Request request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateTicketTypePriceCommand(id, request.Price));

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithTags(Tags.TICKET_TYPES);
    }

    internal sealed class Request
    {
        public decimal Price { get; init; }
    }
}
