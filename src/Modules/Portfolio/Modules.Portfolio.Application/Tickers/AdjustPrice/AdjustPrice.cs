using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Tickers;

public sealed record AdjustPrice(Guid TickerId, string TickerCode, decimal Price) : ICommand;





