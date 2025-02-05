using MongoDB.Driver;
using TagsApi.Models;
using TagsApi.Dtos;

namespace TagsApi.Extensions
{
    public static class QueryableExtensions
    {
        public static IFindFluent<TagDto, TagDto> ApplyQueryParameters(
            this IFindFluent<TagDto, TagDto> query, GetTagQuery tagQuery)
        {
            if (tagQuery is null) return query;

            var sortBuilder = Builders<TagDto>.Sort;
            var sort = sortBuilder.Ascending(t => t.Name); //defaul sorting by name asc

            if (tagQuery.SortBy is not null)
            {
                switch (tagQuery.SortBy)
                {
                    case SortBy.Name:
                        {
                            sort = tagQuery.Order == SortOrder.Desc
                                ? sortBuilder.Descending(t => t.Name)
                                : sortBuilder.Ascending(t => t.Name);
                            break;
                        }
                    case SortBy.Shares:
                        {
                            sort = tagQuery.Order == SortOrder.Desc
                                ? sortBuilder.Descending(t => t.SharesPercentage)
                                : sortBuilder.Ascending(t => t.SharesPercentage);
                            break;
                        }
                }
            }

            query = query.Sort(sort);

            if (tagQuery.Page.HasValue && tagQuery.PageSize.HasValue)
            {
                int skip = (tagQuery.Page.Value - 1) * tagQuery.PageSize.Value;
                query.Skip(skip).Limit(tagQuery.PageSize.Value);
            }

            return query;
        }
    }
}
