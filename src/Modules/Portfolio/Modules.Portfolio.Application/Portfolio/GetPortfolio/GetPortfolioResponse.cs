namespace Modules.Portfolio.Application.Portfolio.GetPortfolio;

public sealed record GetPortfolioResponse(Guid TickerId, int NetQuantity, decimal NetWorth);

