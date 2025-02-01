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
            cfg.CreateMap<StoryDto, BestStoryResponse>()
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Kids.Count))
            .ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.By))
            .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.Url));
        });

        var mapper = config.CreateMapper();

        return mapper;
    }
}