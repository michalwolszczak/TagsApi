using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TagsApi.Dtos
{
    public class TagDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public bool HasSynonyms { get; set; }
        public bool IsModeratonOnly { get; set; }
        public bool IsRequired { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public double SharesPercentage { get; set; }
    }
}
