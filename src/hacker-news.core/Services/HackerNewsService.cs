using System.Text.Json;
using HackerNews.Core.External;
using HackerNews.Core.Models;

namespace HackerNews.Core.Services;

public class HackerNewsService : INewsService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HackerNewsService(IHttpClientFactory httpClientFactory)
    {
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
        var httpClient = _httpClientFactory.CreateClient(HackerNewsConstants.HttpClientName);
        var response = await httpClient.GetAsync($"item/{id}.json");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var story = JsonSerializer.Deserialize<StoryDto>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return story;
    }
}