using MediatR;
using Microsoft.AspNetCore.Http;
using Modules.Orders.Infrastructure.Persistance;

namespace Modules.Orders.Infrastructure.EventualConsistency;

public sealed class EventualConsistencyMiddleware
{
    public const string DomainEventsKey = "DomainEventsKey";
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, OrdersDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CommitAsync();
        }
        catch
        {

        }
        finally
        {
            await transaction.DisposeAsync();
        }

        //context.Response.OnCompleted(async () =>
        //{
        //    try
        //    {
        //        if (context.Items.TryGetValue(DomainEventsKey, out var value) && value is Queue<IDomainEvent> domainEvents)
        //        {
        //            while (domainEvents.TryDequeue(out var nextEvent))
        //            {
        //                await publisher.Publish(nextEvent);
        //            }
        //        }

        //        await transaction.CommitAsync();
        //    }
        //    catch (EventualConsistencyException e)
        //    {
        //        // Handle eventual consistency exceptions
        //    }
        //    finally
        //    {
        //        await transaction.DisposeAsync();
        //    }
        //});

        await _next(context);
    }

}
