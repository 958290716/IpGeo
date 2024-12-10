using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using MongoDB.Driver;

namespace IpGeo.IpLookup.Data
{
    public class IpLookupMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<IpInformation> IpLocationCollection { get; }

        public IpLookupMongoDbContext(IpLookupMongoDbContextSettings ipLookupMongoDbContextSettings)
        {
            var client = new MongoClient(ipLookupMongoDbContextSettings.ConnectString);
            _database = client.GetDatabase(ipLookupMongoDbContextSettings.DatabaseName);
            IpLocationCollection = _database.GetCollection<IpInformation>(
                ipLookupMongoDbContextSettings.CollectionName
            );
        }
    }
}
