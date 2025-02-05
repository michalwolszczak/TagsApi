using TagsApi.Dtos;
using TagsApi.Services;

namespace TagsApi.UnitTests
{
    public class TagSharedCalculationServiceTests
    {
        private readonly ITagSharedCalculationService _tagSharesCalculationService;

        public TagSharedCalculationServiceTests()
        {
            _tagSharesCalculationService = new TagSharedCalculationService();
        }

        [Fact]
        public void CalculateSharedPercentage_WhenListOfTagsIsNotNull_ShouldReturnListOfTagsWithCorrectSharedPercentage()
        {
            //arrange
            var tags = new List<TagDto>
            {
                    new () { Count = 30 },
                    new () { Count = 40 },
                    new () { Count = 50 },
                    new () { Count = 60 },
                    new () { Count = 20 },
            };

            //act
            var result = _tagSharesCalculationService.CalculateSharesPercentage(tags);

            //assert
            var sumSharesPercentage = Math.Round(result.Select(x => x.SharesPercentage).Sum(), 0);

            Assert.Equal(100, sumSharesPercentage);
            Assert.Collection(result,
                tag => Assert.Equal(15, Math.Round(tag.SharesPercentage, 0)),
                tag => Assert.Equal(20, Math.Round(tag.SharesPercentage, 0)),
                tag => Assert.Equal(25, Math.Round(tag.SharesPercentage, 0)),
                tag => Assert.Equal(30, Math.Round(tag.SharesPercentage, 0)),
                tag => Assert.Equal(10, Math.Round(tag.SharesPercentage, 0)));
        }

        [Fact]
        public void CalculateSharedPercentage_WhenListOfTagsIsEmpty_ShouldReturnEmptyList()
        {
            //arrange
            var tags = new List<TagDto>();

            //act
            var result = _tagSharesCalculationService.CalculateSharesPercentage(tags);

            //assert
            Assert.Empty(result);
        }
    }
}