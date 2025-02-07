using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using HackerNews.Models;
using Newtonsoft.Json;
using System.Net;

namespace HackerNews.Tests.Integration
{
    [TestFixture]
    public class NewsControllerIntegrationTests
    {
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            var appFactory = new WebApplicationFactory<Program>();
            _client = appFactory.CreateClient();
        }

        [Test]
        public async Task GetBestStoriesIds_ShouldReturnBestStoriesIds_WhenGoldFlow()
        {
            // Act
            var response = await _client.GetAsync("/News/bestIds");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<int>>(content);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task GetStory_ShouldReturnStoryResponse_WhenGoldFlow()
        {
            // Arrange
            var storyId = 1;

            // Act
            var response = await _client.GetAsync($"/News/{storyId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StoryResponse>(content);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(storyId));
        }

        [Test]
        public async Task GetStory_ShouldReturnNoContent_WhenStoryDoesntExist()
        {
            // Arrange
            var storyId = -1;

            // Act
            var response = await _client.GetAsync($"/News/{storyId}");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task GetStory_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            string storyId = "invalid";

            // Act
            var response = await _client.GetAsync($"/News/{storyId}");

            // Assert
            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetToptStories_ShouldReturnTopStories_WhenGoldFlow()
        {
            // Arrange
            var number = 2;

            // Act
            var response = await _client.GetAsync($"/News/top/{number}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<BestStoryResponse>>(content);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(number));
        }
    }
}