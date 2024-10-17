using FluentResults;
using Microsoft.Extensions.Logging;
using Modules.Orders.Domain.Tickers;
using Modules.Orders.Domain.Tickers.Abstraction;
using SharedKernel.Messaging;

namespace Modules.Orders.Application.Tickers;

internal sealed class AdjustPriceHandler(
    ITickerRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<AdjustPriceHandler> logger) : ICommandHandler<AdjustPrice>
{
    public async Task<Result> Handle(AdjustPrice command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adjust Price {@Ticker} {@Price}", command.TickerCode, command.Price);

        Ticker? ticker = await repository.GetByIdAsync(command.TickerId, cancellationToken);

        if (ticker is null)
        {
            await repository.AddAsync(Ticker.Create(command.TickerId, command.TickerCode, command.Price, DateTime.UtcNow), cancellationToken);
        }
        else
        {
            ticker.UpdatePrice(command.Price);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

