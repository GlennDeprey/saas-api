using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Ticketing.Application.Carts.AddItemToCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Ticketing.Presentation.Carts;

internal sealed class AddToCart: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("carts/add", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(
                new AddItemToCartCommand(
                    request.CustomerId,
                    request.TicketTypeId,
                    request.Quantity));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithTags(Tags.CARTS);
    }

    internal sealed class Request
    {
        public Guid CustomerId { get; init; }

        public Guid TicketTypeId { get; init; }

        public decimal Quantity { get; init; }
    }
}
