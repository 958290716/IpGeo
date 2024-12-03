using IpGeo.IpLookup.Models;
using MongoDB.Driver;

namespace IpGeo
{
    public class MongoDbConnector
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<IpInformation> IpLocationCollection { get; }

        public MongoDbConnector(string connectString, string databaseName)
        {
            var client = new MongoClient(connectString);
            _database = client.GetDatabase(databaseName);
            IpLocationCollection = _database.GetCollection<IpInformation>("IpLocation");
        }

        public void CheckAndCreateCollection(string collectionName)
        {
            var collectionList = _database.ListCollections().ToList();
            var collectionNames = new List<string>();

            collectionList.ForEach(x =>
            {
                collectionNames.Add(x["name"].AsString);
            });
            if (!collectionNames.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }

        public IMongoCollection<IpInformation> GetCollection(string collectionName)
        {
            return _database.GetCollection<IpInformation>(collectionName);
        }
    }
}
