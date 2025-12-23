using System;

namespace MiddleR.Abstractions;

public interface IBaseQuery;
public interface IQuery : IRequest, IBaseQuery;
public interface IQuery<out TResponse> : IRequest<TResponse>, IBaseQuery;


