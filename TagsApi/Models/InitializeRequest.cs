using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TagsApi.Models
{
    [DataContract]
    public class InitializeRequest
    {
        [Range(100, 1000)]
        [DataMember(Name = "minValues")]
        public int MinValues { get; set; }

        [DataMember(Name = "deleteExisting")]
        public bool DeleteExisting { get; set; }
    }
}
