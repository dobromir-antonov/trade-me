using FluentResults;
using MediatR;

namespace SharedKernel.Messaging;

public interface ICommand : IRequest<FluentResults.Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}