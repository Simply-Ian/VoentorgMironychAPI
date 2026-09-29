using MediatR;

namespace Common.Interfaces;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
where TCommand : ICommand<TResponse>
where TResponse: notnull;

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
where TCommand : ICommand;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
where TQuery : IQuery<TResponse>
where TResponse: notnull;