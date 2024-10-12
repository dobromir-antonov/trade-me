using FluentResults;
using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Portfolio.GetPortfolio;

internal sealed class GetPortfolioHandler : IQueryHander<GetPortfolio, GetPortfolioResponse>
{
    public async Task<Result<GetPortfolioResponse>> Handle(GetPortfolio request, CancellationToken cancellationToken)
    {
        return new GetPortfolioResponse();
    }

}