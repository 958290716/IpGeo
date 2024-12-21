using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IpGeo.IpLookup.Models
{
    public class CsvData
    {
        public required string IpStart { get; init; }
        public required string IpEnd { get; init; }
        public required string RegionName { get; init; }
        public required string CountryName { get; init; }
        public required string CityName { get; init; }
    }
}
