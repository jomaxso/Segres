namespace System.Linq;

public interface IOrderedAsyncQueryable : IAsyncQueryable
{

}

public interface IOrderedAsyncQueryable<out T> : IAsyncQueryable<T>, IOrderedAsyncQueryable
{
}