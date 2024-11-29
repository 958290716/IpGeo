using IpGeo.IpLookup.Models;

namespace IpGeo.IpLookup.Data
{
    public class MongoIpInformationRepository : IIpInformationRepository
    {
        public Task CreateAsync(IpInformation ipInformation)
        {
            throw new NotImplementedException();
        }

        public Task<IpInformation?> GetByIpAsync(uint ip)
        {
            throw new NotImplementedException();
        }
    }
}
