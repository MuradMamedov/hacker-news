using AutoMapper;
using HackerNews.Core.Models;

namespace HackerNews.Models;

internal static class Mappings
{
    public static IMapper Mapper { get { return _mapper.Value; } }

    private static Lazy<IMapper> _mapper = new Lazy<IMapper>(GetMapper);

    private static IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {

            cfg.CreateMap<StoryDto, StoryResponse>();
        });

        var mapper = config.CreateMapper();

        return mapper;
    }
}