using Segres.Contracts;

namespace Segres.Behaviors;

public delegate TResult RequestDelegate<TResult>(IRequest<TResult> request);