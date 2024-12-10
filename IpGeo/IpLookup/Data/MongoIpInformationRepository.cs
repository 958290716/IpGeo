using IpGeo.IpLookup.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IpGeo.IpLookup.Data
{
    public class MongoIpInformationRepository(IpLookupMongoDbContext ipLookupMongoDbContext)
        : IIpInformationRepository
    {
        public IMongoCollection<IpInformation> IpInformation =>
            ipLookupMongoDbContext.IpLocationCollection;

        public async Task CreateAsync(IpInformation ipInformation)
        {
            await IpInformation.InsertOneAsync(ipInformation);
        }

        public async Task<IpInformation?> GetByIpAsync(uint ip)
        {
            return await IpInformation
                .AsQueryable()
                .Where(x => x.IpStart <= ip && x.IpEnd >= ip)
                .FirstOrDefaultAsync();
        }
    }
}
