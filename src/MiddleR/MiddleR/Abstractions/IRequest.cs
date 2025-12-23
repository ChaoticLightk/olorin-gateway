namespace MiddleR.Abstractions;

public interface IRequestBase;
public interface IRequest : IRequestBase;
public interface IRequest<out TResponse> : IRequest;

