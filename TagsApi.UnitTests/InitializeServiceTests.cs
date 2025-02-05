using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using TagsApi.Dtos;
using TagsApi.Models;
using TagsApi.Services;

namespace TagsApi.UnitTests
{
    public class InitializeServiceTests
    {
        private readonly IInitializeService _initializeService;
        private readonly Mock<ITagRepoService> _tagRepoServiceMock;
        private readonly Mock<ITagService> _tagServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITagSharedCalculationService> _tagSharedCalculationServiceMock;

        public InitializeServiceTests()
        {
            _tagRepoServiceMock = new Mock<ITagRepoService>();
            _tagServiceMock = new Mock<ITagService>();
            _mapperMock = new Mock<IMapper>();
            _tagSharedCalculationServiceMock = new Mock<ITagSharedCalculationService>();

            _initializeService = new InitializeService(
                _tagServiceMock.Object,
                _tagRepoServiceMock.Object, 
                _mapperMock.Object,
                _tagSharedCalculationServiceMock.Object);
        }

        [Fact]
        public async Task InitializeAsync_WhenGetAsyncReturnError_ShouldReturnProblemDetails()
        {
            //arrange
            var problemDetails = new ProblemDetails() { Title = "Error occured" };

            _tagServiceMock.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(Result.Failure<GetTagResponse, ProblemDetails>(problemDetails));

            //act
            var result = await _initializeService.InitializeAsync(new InitializeRequest());

            //assert
            Assert.True(result.IsFailure);
            Assert.Equal(problemDetails, result.Error);
        }

        [Fact]
        public async Task InitializeAsync_WhenDeleteExistingIsTrue_CallsDeleteAllAsync()
        {
            //arrange
            var request = new InitializeRequest { DeleteExisting = true };

            _tagServiceMock.Setup(s => s.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(Result.Success<GetTagResponse, ProblemDetails>(new GetTagResponse(){ Items = new List<Tag>() }));
            _mapperMock.Setup(m => m.Map<List<TagDto>>(It.IsAny<List<Tag>>()))
                .Returns(new List<TagDto>());

            //act
            await _initializeService.InitializeAsync(request);

            //assert
            _tagRepoServiceMock.Verify(r => r.DeleteAllAsync(), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_WhenSuccess_CallsInsertManyAsync()
        {
            //arrange
            var tags = new List<Tag> { new Tag() };
            var tagDtos = new List<TagDto> { new TagDto() { Name = "c#" } };
            _tagServiceMock.Setup(s => s.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(Result.Success<GetTagResponse, ProblemDetails>(new GetTagResponse() { Items = tags }));
            _mapperMock.Setup(m => m.Map<List<TagDto>>(tags)).Returns(tagDtos);

            //act
            await _initializeService.InitializeAsync(new InitializeRequest());

            //assert
            _tagRepoServiceMock.Verify(r => r.InsertManyAsync(tagDtos), Times.Once);
        }
    }
}
