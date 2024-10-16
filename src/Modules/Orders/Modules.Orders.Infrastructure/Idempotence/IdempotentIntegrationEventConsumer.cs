using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging;

namespace Modules.Orders.Infrastructure.Idempotence;

public class IdempotentIntegrationEventConsumer<T>(
    IIntegrationEventHandler<T> handler,
    ILogger<IdempotentIntegrationEventConsumer<T>> logger) : IConsumer<T> where T : class, IIntegrationEvent
{

    public async Task Consume(ConsumeContext<T> context)
    {
        try
        {
            logger.LogInformation(typeof(T));
            // check if message is already processed

            if (handler is null)
            {
                logger.LogError("Type of \\'{Type}\\' not found in event types", typeof(T));
                throw new ArgumentException($"There is no consumer for {typeof(T)}");
            }

            await handler.Handle(context.Message, context.CancellationToken);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }
}