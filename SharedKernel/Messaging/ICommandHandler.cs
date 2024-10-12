using FluentResults;
using MediatR;

namespace SharedKernel.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, FluentResults.Result> where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
}
