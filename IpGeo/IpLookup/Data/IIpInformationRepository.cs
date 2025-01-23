using IpGeo.IpLookup.Models;

namespace IpGeo.IpLookup.Data;

public interface IIpInformationRepository
{
    public Task CreateAsync(IpInformation ipInformation);
    public Task<IpInformation?> GetByIpAsync(uint ip);
    public Task CreateManyAsync(List<IpInformation> ipInformation);
}
