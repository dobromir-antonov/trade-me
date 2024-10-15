using MediatR;
using Microsoft.AspNetCore.Http;
using Modules.Orders.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.Infrastructure.EventualConsistency;

public sealed class EventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, OrdersDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        context.Response.OnCompleted(async () =>
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }

}
