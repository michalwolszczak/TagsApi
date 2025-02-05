using MongoDB.Driver;
using Moq;
using TagsApi.Dtos;
using TagsApi.Services;

namespace TagsApi.UnitTests
{
    public class TagRepoServiceTests
    {
        private ITagRepoService _tagRepoService;
        private readonly Mock<IMongoCollection<TagDto>> _mongoCollectionMock;

        public TagRepoServiceTests()
        {
            _mongoCollectionMock = new Mock<IMongoCollection<TagDto>>();
        }

        [Fact]
        public async Task InsertManyAsync_WhenNoTagsAreProvided_ShouldNotCallInsert()
        {
            //arrange            

            var tagRepoService = new TagRepoService(_mongoCollectionMock.Object);
            var newTags = new List<TagDto>();

            //act
            await tagRepoService.InsertManyAsync(newTags);

            //assert
            _mongoCollectionMock.Verify(x => x.InsertManyAsync(It.IsAny<List<TagDto>>(), It.IsAny<InsertManyOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
