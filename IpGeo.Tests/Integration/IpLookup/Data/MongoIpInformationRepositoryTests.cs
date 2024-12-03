using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    public class MongoIpInformationRepositoryTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        MongoIpInformationRepository Repository => new();

        [Fact]
        public async Task CreateIp_ShouldSuccess()
        {
            // 创建 IpInformation 实例
            var ipInfo = new IpInformation
            {
                IpStart = ipStart,
                IpEnd = ipEnd,
                RegionName = regionName,
                CountryName = countryName,
                CityName = cityName,
            };
            await Repository.CreateAsync(ipInfo);
        }

        [Fact]
        public async Task LookupIp_WhenValidIp_ShouldReturnIpInformation()
        {
            IpInformation? IpInfo = await Repository.GetByIpAsync(ip);
            Assert.NotNull(IpInfo);
            Assert.Equal(result, IpInfo.CityName);
        }

        [Fact]
        public async Task LookupIp_WhenNoRecord_ShouldReturnNull()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => Repository.GetByIpAsync(ip));
        }
    }
}
