using Microsoft.AspNetCore.Routing;

namespace SharedKernel.Application;
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
