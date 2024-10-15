using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel.Application;
using SharedKernel.Results;

namespace Modules.Portfolio.Application.Portfolio.GetPortfolio;

internal sealed class GetPortfolioEndpoint : IEndpoint
{

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapGet("{userId:guid}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
           {
               Result<GetPortfolioResponse[]> result = await sender.Send(new GetPortfolio(userId), cancellationToken);

               return result.Match(
                   response => Results.Ok(response),
                   errors => errors.ToProblem()
               );
           });
    }
}
