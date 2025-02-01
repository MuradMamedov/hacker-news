using HackerNews.Models;
using Microsoft.AspNetCore.Mvc;
using HackerNews.Core.Services;

namespace HackerNews.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly ILogger<NewsController> _logger;
    private readonly INewsService _hackerNewsService;

    public NewsController(ILogger<NewsController> logger, INewsService hackerNewsService)
    {
        _logger = logger;
        _hackerNewsService = hackerNewsService;
    }

    [HttpGet(Name = nameof(BestStories))]
    public Task<IEnumerable<int>> BestStories()
    {
        return _hackerNewsService.GetBestStoriesAsync();
    }

    [HttpGet("{id}", Name = nameof(GetStory))]
    public async Task<StoryResponse> GetStory(int id)
    {
        var dto = await _hackerNewsService.GetStoryAsync(id);

        return Mappings.Mapper.Map<StoryResponse>(dto);
    }
}
