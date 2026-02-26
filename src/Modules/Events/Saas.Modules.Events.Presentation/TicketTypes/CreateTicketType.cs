using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.TicketTypes.CreateTicketType;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.TicketTypes;

internal class CreateTicketType: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("ticket-types", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(new CreateTicketTypeCommand(
                request.EventId,
                request.Name,
                request.Price,
                request.Currency,
                request.Quantity));

            return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.TICKET_TYPES);
    }

    internal sealed class Request
    {
        public Guid EventId { get; init; }

        public required string Name { get; init; }

        public decimal Price { get; init; }

        public required string Currency { get; init; }

        public decimal Quantity { get; init; }
    }
}
