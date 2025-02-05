using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;
using System.Net.Http.Json;
using System.Text.Json;
using TagsApi.Dtos;
using TagsApi.Models;

namespace TagsApi.IntegrationTests
{
    public class TagsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TagsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTags_ReturnsSuccessAndCorrectContentType()
        {
            //act
            var response = await _client.GetAsync("/api/tags");

            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetTags_ReturnsExpectedTags()
        {
            //act
            var response = await _client.GetAsync("/api/tags");
            var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>();

            //assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.Equal(1000, tags.Count);
        }

        [Fact]
        public async Task GetTags_WhenGetTagQueryNotNull_ReturnsExpectedTags()
        {
            //arrange
            var queryBuilder = new QueryBuilder()
            {
                { "pageSize", "10" },
                { "page", "1" }
            };

            //act
            var response = await _client.GetAsync($"/api/tags{queryBuilder}");
            var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>();

            //assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.Equal(10, tags.Count);
        }

        [Fact]
        public async Task InitializeTagsAsync_WhenInitializeRequestIsNull_ReturnsBadRequest()
        {
            //act
            var response = await _client.PostAsync($"/api/tags/initialize", new StringContent("{test}", System.Text.Encoding.UTF8, "application/json"));

            //assert
            Assert.True(!response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task InitializeTagsAsync_WhenInitializeRequestIsNotNull_ReturnsSuccess()
        {
            //arrange
            var initializeRequest = new InitializeRequest()
            {
                MinValues = 100,
                DeleteExisting = true,
            };

            //act
            var responseInitialize = await _client.PostAsync($"/api/tags/initialize", 
                new StringContent(JsonSerializer.Serialize(initializeRequest), System.Text.Encoding.UTF8, "application/json"));
            var responseGetAsync = await _client.GetAsync($"/api/tags");
            var tags = await responseGetAsync.Content.ReadFromJsonAsync<List<TagDto>>();


            //assert
            responseInitialize.EnsureSuccessStatusCode();
            responseGetAsync.EnsureSuccessStatusCode();

            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.Equal(100, tags.Count);
        }
    }
}
