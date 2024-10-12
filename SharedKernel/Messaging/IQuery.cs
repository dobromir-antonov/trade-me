using FluentResults;
using MediatR;

namespace SharedKernel.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}