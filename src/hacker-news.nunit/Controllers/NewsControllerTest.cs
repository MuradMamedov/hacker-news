using NUnit.Framework;
using Microsoft.Extensions.Logging;
using HackerNews.Core.Services;
using Moq;
using HackerNews.Controllers;
using HackerNews.Core.Models;
using HackerNews.Models;

namespace HackerNews.Tests.Controllers
{
    [TestFixture]
    public class NewsControllerTest
    {
        private Mock<ILogger<NewsController>> _loggerMock;
        private Mock<INewsService> _newsServiceMock;
        private NewsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<NewsController>>();
            _newsServiceMock = new Mock<INewsService>();
            _controller = new NewsController(_loggerMock.Object, _newsServiceMock.Object);
        }

        [Test]
        public async Task GetBestStoriesIds_ShouldReturnsBestStories_WhenGoldFlow()
        {
            // Arrange
            var bestStories = new List<int> { 1, 2, 3 };
            _newsServiceMock.Setup(service => service.GetBestStoriesAsync()).ReturnsAsync(bestStories);

            // Act
            IEnumerable<int> result = await _controller.GetBestStoriesIds();

            // Assert
            Assert.That(result, Is.EqualTo(bestStories));
        }

        [Test]
        public async Task GetStory_ShouldReturnsStoryResponse_WhenGoldFlow()
        {
            // Arrange
            var storyId = 1;
            var storyDto = new StoryDto { Id = storyId, Title = "Test Story" };
            var storyResponse = new StoryResponse { Id = storyId, Title = "Test Story" };
            _newsServiceMock.Setup(service => service.GetStoryAsync(storyId)).ReturnsAsync(storyDto);

            // Act
            var result = await _controller.GetStory(storyId);

            // Assert
            Assert.That(result.Id, Is.EqualTo(storyResponse.Id));
            Assert.That(result.Title, Is.EqualTo(storyResponse.Title));
            Assert.That(result.By, Is.EqualTo(storyResponse.By));
        }

        [Test]
        public async Task GetToptStories_ReturnsBestStoryResponses_WhenGoldFlow()
        {
            // Arrange
            var number = 2;
            var bestStories = new List<int> { 1, 2, 3 };
            var storyDtos = new List<StoryDto>
            {
                new StoryDto { Id = 1, Score = 100 },
                new StoryDto { Id = 2, Score = 200 },
                new StoryDto { Id = 3, Score = 150 }
            };
            var bestStoryResponses = storyDtos.OrderByDescending(s => s.Score).Take(number).Select(dto => new BestStoryResponse { Title = dto.Title, Score = dto.Score }).ToList();
            _newsServiceMock.Setup(service => service.GetBestStoriesAsync()).ReturnsAsync(bestStories);
            _newsServiceMock.Setup(service => service.GetStoryAsync(It.IsAny<int>())).ReturnsAsync((int id) => storyDtos.First(dto => dto.Id == id));

            // Act
            var result = await _controller.GetToptStories(number);

            // Assert
            Assert.That(number, Is.EqualTo(result.Count()));
            Assert.That(bestStoryResponses.First().Title, Is.EqualTo(result.First().Title));
            Assert.That(bestStoryResponses.First().Score, Is.EqualTo(result.First().Score));
        }
    }
}