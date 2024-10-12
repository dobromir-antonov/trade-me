namespace Modules.Portfolio.Application.Portfolio.GetPortfolio;

internal sealed record GetPortfolioResponse();

internal sealed record ClientResponse(Guid Id, string Name, string ContactPerson, string Email, string PhoneNumber);

