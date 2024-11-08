using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectUnknown.AspNetCore.DataProtection.MongoDB
{
    internal class MongoXmlDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("xml")]
        public string Xml { get; set; }
    }
}
