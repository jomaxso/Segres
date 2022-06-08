using System.Linq.Expressions;

namespace System.Linq;


public interface IAsyncQueryable<out T> : IAsyncEnumerable<T>, IAsyncQueryable
{
}

public interface IAsyncQueryable 
{
    Type ElementType { get; }
    Expression Expression { get; } 
    IAsyncQueryProvider Provider { get; }
}

// IAsyncEnumerable<T> AsAsyncEnumerable();
// IQueryable<T> AsQueryable();