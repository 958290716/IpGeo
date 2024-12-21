using IpGeo.IpLookup.Models;
using MongoDB.Driver;

namespace IpGeo.Api
{
    public class GetDataToDatabaseApi
    {
        public string address =
            "https://cdn.jsdelivr.net/npm/@ip-location-db/geolite2-city/geolite2-city-ipv4.csv.gz";
        private readonly List<IpInformation> ipInformation = new();
    }
}
