using AutoMapper;

namespace SharedKernel.Mappers;

public static class BaseMapper<TSource, TDestination>
{
    public static Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<TSource, TDestination>().ReverseMap();
    }));

    public static TDestination Map(TSource source)
    {
        return mapper.Map<TDestination>(source);
    }

    public static List<TDestination> Map(List<TSource> source)
    {
        return mapper.Map<List<TDestination>>(source);
    }
}