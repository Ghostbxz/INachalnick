using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProgramUtilities.Mongo
{
    public class MongoBaseDocument
    {
        [BsonId, BsonIgnoreIfNull, BsonIgnoreIfDefault, JsonIgnore] public ObjectId? Id { get; set; }
        private string? _validType;
        [BsonElement("_t"), BsonIgnoreIfNull, BsonIgnoreIfDefault, JsonProperty("_t"), SwaggerExclude] public string? MongoClassType { get => string.IsNullOrEmpty(_validType) ? null : _validType; set => _validType = string.IsNullOrEmpty(value) ? null : value; }
        [BsonIgnore, JsonIgnore] public string? IdStr => Id?.ToString();
    }
}
