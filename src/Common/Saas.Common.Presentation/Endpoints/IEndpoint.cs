using Microsoft.AspNetCore.Routing;

namespace Saas.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
