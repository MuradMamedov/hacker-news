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

    [HttpGet("bestIds", Name = nameof(GetBestStoriesIds))]
    public Task<IEnumerable<int>> GetBestStoriesIds()
    {
        return _hackerNewsService.GetBestStoriesAsync();
    }

    [HttpGet("{id}", Name = nameof(GetStory))]
    public async Task<StoryResponse> GetStory(int id)
    {
        var dto = await _hackerNewsService.GetStoryAsync(id);

        return Mappings.Mapper.Map<StoryResponse>(dto);
    }

    [HttpGet("top/{number}", Name = nameof(GetToptStories))]
    public async Task<IEnumerable<BestStoryResponse>> GetToptStories(int number)
    {
        var bestStories = await _hackerNewsService.GetBestStoriesAsync();
        var tasks = bestStories.Select(id => _hackerNewsService.GetStoryAsync(id));
        var stories = await Task.WhenAll(tasks);

        return stories.OrderByDescending(s => s.Score).Take(number).Select(Mappings.Mapper.Map<BestStoryResponse>);
    }
}
