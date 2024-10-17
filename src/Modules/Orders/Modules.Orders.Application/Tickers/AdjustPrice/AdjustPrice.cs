using SharedKernel.Messaging;

namespace Modules.Orders.Application.Tickers;

public sealed record AdjustPrice(Guid TickerId, string TickerCode, decimal Price) : ICommand;





