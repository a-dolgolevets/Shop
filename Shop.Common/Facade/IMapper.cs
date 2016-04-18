namespace Shop.Common.Facade
{
    public interface IMapper
    {
        TDest Map<TDest>(object source);

        TDest Map<TSource, TDest>(TSource source);

        TDest Map<TSource, TDest>(TSource source, TDest dest);  
    }
}