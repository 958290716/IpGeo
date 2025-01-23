using IpGeo.IpLookup.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IpGeo.IpLookup.Data
{
    // DataService
    public class MongoIpInformationRepository(IpLookupMongoDbContext ipLookupMongoDbContext)
        : IIpInformationRepository
    {
        public IMongoCollection<IpInformation> IpInformation =>
            ipLookupMongoDbContext.IpLocationCollection;

        public async Task CreateAsync(IpInformation ipInformation)
        {
            await IpInformation.InsertOneAsync(ipInformation);
        }

        public async Task CreateManyAsync(List<IpInformation> ipInformation)
        {
            await IpInformation.InsertManyAsync(ipInformation);
        }

        public async Task<IpInformation?> GetByIpAsync(uint ip)
        {
            return await IpInformation
                .AsQueryable()
                .Where(x => x.IpStart <= ip && x.IpEnd >= ip)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteByIpAsync(uint startIp)
        {
            var result = await IpInformation.DeleteOneAsync(x => x.IpStart == startIp);
            if (result.DeletedCount == 1)
            {
                return result.DeletedCount > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
