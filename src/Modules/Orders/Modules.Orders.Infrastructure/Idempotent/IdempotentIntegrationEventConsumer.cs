using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging;

namespace Modules.Orders.Infrastructure.Idempotent;

public sealed class IdempotentIntegrationEventConsumer<T>(
    IPublisher publisher,
    ILogger<IdempotentIntegrationEventConsumer<T>> logger) : IConsumer<T> where T : class, IIntegrationEvent
{

    public async Task Consume(ConsumeContext<T> context)
    {
        try
        {
            // check if message is already processed

            await publisher.Publish(context.Message, context.CancellationToken);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }
}