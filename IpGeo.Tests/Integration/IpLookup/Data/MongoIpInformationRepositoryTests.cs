using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Testcontainers.MongoDb;
using MongoDB.Driver;
using Xunit.Abstractions;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    public class MongoIpInformationRepositoryTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        //MongoIpInformationRepository Repository => new();

        //[Fact]
        //public async Task CreateIp_ShouldSuccess()
        //{
        //    // 创建 IpInformation 实例
        //    var ipInfo = new IpInformation
        //    {
        //        IpStart = 123,
        //        IpEnd = 345,
        //        RegionName = "liwan",
        //        CountryName = "china",
        //        CityName = "gz",
        //    };
        //    await Repository.CreateAsync(ipInfo);
        //}

        //[Fact]
        //public async Task LookupIp_WhenValidIp_ShouldReturnIpInformation()
        //{
        //    IpInformation? IpInfo = await Repository.GetByIpAsync(125);
        //    Assert.NotNull(IpInfo);
        //    Assert.Equal("gz", IpInfo.CityName);
        //}

        //[Fact]
        //public async Task LookupIp_WhenNoRecord_ShouldReturnNull()
        //{
        //    await Assert.ThrowsAsync<InvalidOperationException>(() => Repository.GetByIpAsync(1000));
        //}

        
        
        //[Fact]
        //public async Task Debug_MongoDB()
        //{

        //}
        
      
        
    }
}
