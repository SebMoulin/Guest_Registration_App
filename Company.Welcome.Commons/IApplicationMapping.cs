namespace Company.Welcome.Commons
{
    public interface IApplicationMapping
    {
        void Configure();
        TDestination Map<TSource, TDestination>(TSource source);
    }
}