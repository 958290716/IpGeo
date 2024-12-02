using IpGeo.IpLookup.Models;
using MongoDB.Driver;
using IpGeo;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace IpGeo.IpLookup.Data
{
    public class MongoIpInformationRepository : IIpInformationRepository
    {
        public async Task CreateAsync(IpInformation ipInformation)
        {
            MongoDbConnector mongoDbConnector = new("mongodb://localhost:27017", "jovial_agnesi");
            mongoDbConnector.CheckAndCreateCollection("IpLocation");
            var collationName = mongoDbConnector.GetCollection("IpLocation");
            await collationName.InsertOneAsync(ipInformation);
        }

        
        public async Task<IpInformation?> GetByIpAsync(uint ip)
        {
            MongoDbConnector mongoDbConnector = new("mongodb://localhost:27017", "jovial_agnesi");
            mongoDbConnector.CheckAndCreateCollection("IpLocation");
            var collationName = mongoDbConnector.GetCollection("IpLocation");
            return await collationName.AsQueryable().Where(x => x.IpStart <= ip && x.IpEnd >= ip).FirstAsync();

        }
    }
}
