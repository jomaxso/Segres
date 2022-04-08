namespace MicrolisR.Mapping;

public interface IMapperDefinition<in TSource, out TTarget> : IMapperHandler
{
    TTarget Map(TSource mappable);
}