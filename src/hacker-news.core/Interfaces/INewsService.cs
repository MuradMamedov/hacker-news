using HackerNews.Core.Models;

namespace HackerNews.Core.Services;

public interface INewsService
{
    Task<IEnumerable<int>> GetBestStoriesAsync();
    Task<StoryDto> GetStoryAsync(int id);
}