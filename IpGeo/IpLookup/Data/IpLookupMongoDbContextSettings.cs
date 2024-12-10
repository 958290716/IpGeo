namespace IpGeo.IpLookup.Data
{
    public record IpLookupMongoDbContextSettings(
        string ConnectString,
        string DatabaseName,
        string CollectionName
    ) { }
}
