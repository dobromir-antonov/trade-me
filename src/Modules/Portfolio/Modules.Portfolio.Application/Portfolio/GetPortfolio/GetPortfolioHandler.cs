using FluentResults;
using Microsoft.EntityFrameworkCore;
using Modules.Portfolio.Application.Abstraction;
using SharedKernel.Messaging;

namespace Modules.Portfolio.Application.Portfolio.GetPortfolio;

internal sealed class GetPortfolioHandler(IPortfolioReadOnlyDbContext context) : IQueryHander<GetPortfolio, GetPortfolioResponse[]>
{

    public async Task<Result<GetPortfolioResponse[]>> Handle(GetPortfolio request, CancellationToken cancellationToken)
    {
        var result = await context.UserPortfolios
            .FromSqlInterpolated($@"
                SELECT 	o.ticker_id as TickerId
                ,		SUM(CASE WHEN o.side = 0 THEN o.quantity ELSE -o.quantity END) AS NetQuantity
                ,		SUM(CASE WHEN o.side = 0 THEN o.quantity * o.price ELSE 0 END) - SUM(CASE WHEN o.side = 1 THEN o.quantity * o.price ELSE 0 END) AS NetWorth
                FROM 	portfolio.orders o

		                INNER JOIN 	portfolio.tickers t
		                ON			t.id = o.ticker_id
				
                WHERE	o.user_id = {request.UserId}
		
                GROUP BY
		                o.ticker_id")
            .ToArrayAsync();

        return result;
    }

}