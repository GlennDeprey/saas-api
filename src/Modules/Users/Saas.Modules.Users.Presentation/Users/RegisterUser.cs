using Saas.Common.Domain;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Users.Presentation.Users;

internal sealed class RegisterUser: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (Request request, ISender sender) =>
        {
            var result = await sender.Send(new RegisterUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public required string Email { get; init; }

        public required string Password { get; init; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }
    }
}
