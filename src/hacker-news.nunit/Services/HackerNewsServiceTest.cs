using System.Net;
using System.Text.Json;
using HackerNews.Core.External;
using HackerNews.Core.Models;
using HackerNews.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace HackerNews.Tests.Services
{
    [TestFixture]
    public class HackerNewsServiceTest
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private HackerNewsService _hackerNewsService;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _hackerNewsService = new HackerNewsService(_httpClientFactoryMock.Object, _memoryCacheMock.Object);
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldReturnBestStories_WhenGoldFlow()
        {
            // Arrange
            var bestStories = new List<int> { 1, 2, 3 };
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(bestStories))
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(HackerNewsConstants.BaseAddress) };
            _httpClientFactoryMock.Setup(f => f.CreateClient(HackerNewsConstants.HttpClientName)).Returns(httpClient);

            // Act
            var result = await _hackerNewsService.GetBestStoriesAsync();

            // Assert
            Assert.That(result, Is.EqualTo(bestStories));
        }

        [Test]
        public async Task GetStoryAsync_ShouldReturnStory_WhenNotInCache()
        {
            // Arrange
            var storyId = 1;
            var story = new StoryDto { Id = storyId, Title = "Test Story" };
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(story))
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object) { BaseAddress = new Uri(HackerNewsConstants.BaseAddress) };
            _httpClientFactoryMock.Setup(f => f.CreateClient(HackerNewsConstants.HttpClientName)).Returns(httpClient);

            object cacheEntry = null;
            _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out cacheEntry)).Returns(false);
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            // Act
            var result = await _hackerNewsService.GetStoryAsync(storyId);

            // Assert
            Assert.That(result.Id, Is.EqualTo(story.Id));
        }

        [Test]
        public async Task GetStoryAsync_ShouldReturnStory_WhenInCache()
        {
            // Arrange
            int storyId = 1;
            object story = new StoryDto { Id = storyId, Title = "Test Story" };

            _memoryCacheMock
                      .Setup(x => x.TryGetValue(It.IsAny<object>(), out story))
                      .Returns(true);

            // Act
            var result = await _hackerNewsService.GetStoryAsync(storyId);

            // Assert
            Assert.That(result, Is.EqualTo(story));
        }
    }
}