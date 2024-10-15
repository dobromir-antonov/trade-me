using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Portfolio.GetPortfolio;

internal sealed record GetPortfolio(Guid UserId) : IQuery<GetPortfolioResponse[]>;
