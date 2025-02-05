using System.Runtime.Serialization;

namespace TagsApi.Models
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "has_synonyms")]
        public bool HasSynonyms { get; set; }
        
        [DataMember(Name = "is_moderator_only")]
        public bool IsModeratonOnly { get; set; }
        
        [DataMember(Name = "is_required")]
        public bool IsRequired { get; set; }
        
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}