using FluentResults;
using MediatR;

namespace SharedKernel.Messaging;

public interface IQueryHander<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}
