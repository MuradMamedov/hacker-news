using System.Text.Json;
using hacker_news.Models;
using Microsoft.AspNetCore.Mvc;

namespace hacker_news.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsController(ILogger<NewsController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = nameof(BestStories))]
        public IEnumerable<int> BestStories()
        {
            var httpClient = _httpClientFactory.CreateClient(HackerNewsConstants.HttpClientName);
            var response = httpClient.GetAsync(HackerNewsConstants.Endpoints.BestStories).Result;
            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var bestStories = JsonSerializer.Deserialize<IEnumerable<int>>(responseBody);

            return bestStories;
        }

        [HttpGet("{id}", Name = nameof(GetStory))]
        public async Task<StoryResponse> GetStory(int id)
        {
            var httpClient = _httpClientFactory.CreateClient(HackerNewsConstants.HttpClientName);
            var response = await httpClient.GetAsync($"item/{id}.json");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var story = JsonSerializer.Deserialize<StoryResponse>(responseBody);

            return story;
        }
    }
}
