using System.Text.Json;
using HackerNews.Core.External;
using HackerNews.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Core.Services;

public class HackerNewsService : INewsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;

    private const int CacheExpirationInMinutes = 15;

    public HackerNewsService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<int>> GetBestStoriesAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(HackerNewsConstants.HttpClientName);
        var response = await httpClient.GetAsync(HackerNewsConstants.Endpoints.BestStories);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var bestStories = JsonSerializer.Deserialize<IEnumerable<int>>(responseBody);

        return bestStories;
    }

    public async Task<StoryDto> GetStoryAsync(int id)
    {
        StoryDto story;
        if (!_memoryCache.TryGetValue(id, out story))
        {
            var httpClient = _httpClientFactory.CreateClient(HackerNewsConstants.HttpClientName);
            var response = await httpClient.GetAsync($"item/{id}.json");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            story = JsonSerializer.Deserialize<StoryDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _memoryCache.Set(id, story, TimeSpan.FromMinutes(5));
        }
        return story;
    }
}