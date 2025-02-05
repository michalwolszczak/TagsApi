using System.Runtime.Serialization;

namespace TagsApi.Models
{
    [DataContract]
    public class GetTagQuery
    {
        [DataMember(Name = "order")]
        public SortOrder? Order { get; set; }

        [DataMember(Name = "sortBy")]
        public SortBy? SortBy { get; set; }

        [DataMember(Name = "page")]
        public int? Page { get; set; }

        [DataMember(Name = "pageSize")]
        public int? PageSize { get; set; }
    }

    public enum SortBy
    {
        Name,
        Shares
    }

    public enum SortOrder 
    {
        Asc,
        Desc
    }
}