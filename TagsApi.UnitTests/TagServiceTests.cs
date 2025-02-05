using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using TagsApi.Common;
using TagsApi.Models;
using TagsApi.Services;

namespace TagsApi.UnitTests
{
    public class TagServiceTests
    {
        private ITagService _tagService;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IProblemDetailsFactory> _problemDetailsFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public TagServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task GetAsync_WhenRequestFailed_ShouldReturnProblemDetails()
        {
            //arrane                        
            var minValues = 100;
            var problemDetails = new ProblemDetails() { Detail = "error", Title = "error", Status = 400 };

            _problemDetailsFactoryMock.Setup(x => x.CreateProblemDetails(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(problemDetails);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://test.com") };

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _tagService = new TagService(_httpClientFactoryMock.Object, _problemDetailsFactoryMock.Object);

            //act
            var result = await _tagService.GetAsync(minValues);

            //assert
            Assert.True(result.IsFailure);
            Assert.Equal(problemDetails, result.Error);
        }

        [Fact]
        public async Task GetAsync_WhenRequestSuccess_ShouldReturnListOfTags()
        {
            //arrane
            int minValues = 200;
            var getTagResponse = new GetTagResponse() { Items = new List<Tag> { new Tag() } }; 

            var jsonResponse = JsonSerializer.Serialize(getTagResponse);

            _httpMessageHandlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("http://test.com") };

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _tagService = new TagService(_httpClientFactoryMock.Object, _problemDetailsFactoryMock.Object);

            //act
            var result = await _tagService.GetAsync(minValues);

            //assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value.Items);
            Assert.Equal(2, result.Value.Items?.Count);

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
