using IpGeo.IpLookup.Models;

namespace IpGeo.Services
{
    public interface IRepositoryService
    {
        public Task<IpInformation> GetDataByIpService(uint ipStart);
        public Task CreateService(IpInformation ipInformation);
    }
}
