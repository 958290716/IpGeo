using System.Security.Cryptography.X509Certificates;

namespace IpGeo.IpLookup.Data
{
    public class IpLookupMongoDbContextSettings
    {
        public string ConnectString { get; init; } = null!;
        public string DatabaseName { get; init; } = null!;
        public string CollectionName { get; init; } = null!;
    }
}
