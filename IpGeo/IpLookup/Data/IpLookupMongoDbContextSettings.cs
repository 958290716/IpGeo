using System.Security.Cryptography.X509Certificates;

namespace IpGeo.IpLookup.Data
{
    public record IpLookupMongoDbContextSettings(
        string ConnectString,
        string DatabaseName,
        string CollectionName
    ) { }
}
