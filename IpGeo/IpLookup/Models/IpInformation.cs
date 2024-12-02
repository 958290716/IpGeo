using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IpGeo.IpLookup.Models
{
    public record IpInformation
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public required uint Id {  get; set; }
        public required uint IpStart { get; init; }
        public required uint IpEnd { get; init; }
        public required string RegionName { get; init; }
        public required string CountryName { get; init; }
        public required string CityName { get; init; }
    }
}
