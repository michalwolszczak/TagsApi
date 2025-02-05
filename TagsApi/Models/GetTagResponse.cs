using System.Runtime.Serialization;

namespace TagsApi.Models
{
    [DataContract]
    public class GetTagResponse
    {
        [DataMember(Name = "items")]
        public List<Tag> Items { get; set; }
    }
}