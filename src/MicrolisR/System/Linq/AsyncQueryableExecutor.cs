using System.Linq.Expressions;

namespace System.Linq;

internal class AsyncQueryableExecutor<T>
{
    private readonly Expression _expression;
    private Func<CancellationToken, ValueTask<T>>? _executionContext;
    
    public AsyncQueryableExecutor(Expression expression)
    {
        this._expression = expression;
    }
    
    internal ValueTask<T> ExecuteAsync(CancellationToken cancellationToken)
    {
        this._executionContext ??= CreateExecutionContext(this._expression);
        return this._executionContext.Invoke(cancellationToken);
    }

    private static Func<CancellationToken, ValueTask<T>> CreateExecutionContext(Expression expression)
    {
        var rewriter = new AsyncEnumerableRewriter();
        var body = rewriter.Visit(expression);
        var parameter = Expression.Parameter(typeof(CancellationToken));
        var expressionLambda = Expression.Lambda<Func<CancellationToken, ValueTask<T>>>(body, parameter);
        return expressionLambda.Compile();
    }
}