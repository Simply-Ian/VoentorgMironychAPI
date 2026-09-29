using MediatR;

namespace Common.Interfaces;

public interface ICommand<out TResult> : IRequest<TResult> where TResult : notnull;

public interface ICommand : ICommand<Unit>;

public interface IQuery<out TResult> : IRequest<TResult> where TResult: notnull;