using TagsApi.Dtos;

namespace TagsApi.Services
{
    public interface ITagSharedCalculationService
    {
        IList<TagDto> CalculateSharesPercentage(List<TagDto> tags);
    }

    public class TagSharedCalculationService : ITagSharedCalculationService
    {
        public IList<TagDto> CalculateSharesPercentage(List<TagDto> tags)
        {
            var totalCount = tags.Aggregate(0, (sum, tag) => sum + tag.Count);

            tags.ForEach(x => x.SharesPercentage = Math.Round(x.Count * 100.0 / totalCount, 2));

            return tags;
        }
    }
}
