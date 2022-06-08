using System.Linq.Expressions;

namespace System.Linq;

public interface IAsyncQueryProvider
{
    // IAsyncQueryable CreateQuery(Expression expression);
    
    IAsyncQueryable<TElement> CreateQuery<TElement>(Expression expression);
    
    ValueTask<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
}