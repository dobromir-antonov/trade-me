using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel.Application;
using SharedKernel.Results;

namespace Modules.Orders.Application.Orders.PlaceOrder;

internal sealed class PlaceOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("place/{userId:guid}", async (Guid userId, PlaceOrderRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                Result<PlaceOrderResponse> result = await sender.Send(
                    new PlaceOrder(userId, request.TickerId, request.Quantity), 
                    cancellationToken);

                return result.Match(
                    response => Results.Ok(response),
                    errors => errors.ToProblem()
                );
            })
            .DisableAntiforgery();
    }
}

