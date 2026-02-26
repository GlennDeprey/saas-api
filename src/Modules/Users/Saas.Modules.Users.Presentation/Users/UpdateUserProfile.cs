using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Users.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Users.Presentation.Users;

internal sealed class UpdateUserProfile: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{id}/profile", async (Guid id, Request request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateUserCommand(
                id,
                request.FirstName,
                request.LastName));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public required string FirstName { get; init; }

        public required string LastName { get; init; }
    }
}
