namespace MiddleR.Abstractions;

public interface IBaseCommand;
public interface ICommand : IRequest, IBaseCommand;
public interface ICommand<TResponse> : IRequest<TResponse>, IBaseCommand;
