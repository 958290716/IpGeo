using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Microsoft.VisualBasic;
using System.Runtime;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    public class MongoIpInformationRepositoryTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        MongoIpInformationRepository Repository => new();

        [Theory]
        [InlineData(1, 123, 123, "area", "china", "guangzhou")]
        [InlineData(2, 234, 345, "area", "china", "shenzhen")]
        [InlineData(3, 345, 456, "area", "china", "shanghai")]
        public async Task CreateIp_ShouldSuccess1(uint id, uint ipStart, uint ipEnd, string regionName, string countryName, string cityName)
        {
            // 创建 IpInformation 实例
            var ipInfo = new IpInformation
            {
                Id = id,
                IpStart = ipStart,
                IpEnd = ipEnd,
                RegionName = regionName,
                CountryName = countryName,
                CityName = cityName
            };
            await Repository.CreateAsync(ipInfo);
        }


        [Theory]
        [InlineData(255,"shenzhen")]
        [InlineData(366, "shanghai")]
        public async Task LookupIp_WhenValidIp_ShouldReturnIpInformation(uint ip,string result)
        {
            IpInformation? IpInfo =  await Repository.GetByIpAsync(ip);
            Assert.Equal(result, IpInfo.CityName);
        }

        [Theory]
        [InlineData(1000)]
        public async Task LookupIp_WhenNoRecord_ShouldReturnNull(uint ip)
        {
            //IpInformation? IpInfo = await Repository.GetByIpAsync(ip);
            await Assert.ThrowsAsync<InvalidOperationException>(() => Repository.GetByIpAsync(ip));
        }
    }
}
