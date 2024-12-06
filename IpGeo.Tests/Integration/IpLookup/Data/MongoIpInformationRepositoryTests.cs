using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Testcontainers.MongoDb;
using MongoDB.Driver;
using Xunit.Abstractions;
using Docker.DotNet.Models;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    public class MongoDbContainerFixture : IAsyncLifetime
    {
        private readonly MongoDbContainer _mongoDbContainer;
        public string ConnectionString = ""; // => _mongoDbContainer.GetConnectionString();      

        public MongoDbContainerFixture()
        {
            // 创建 MongoDB 容器配置
            _mongoDbContainer = new MongoDbBuilder()
                .WithImage("mongo:latest")
                .Build(); // 默认 MongoDB 端口
        }

        public async Task InitializeAsync()
        {
            // 连接到 MongoDB 实例
            await _mongoDbContainer.StartAsync();
            ConnectionString = _mongoDbContainer.GetConnectionString();
        }

        public async Task DisposeAsync()
        {
            // 测试完成后关闭容器
            if (_mongoDbContainer != null)
            {
                await _mongoDbContainer.StopAsync();
            }
        }
    }

    public class IpInformationTests : IClassFixture<MongoDbContainerFixture>
    {
        public MongoIpInformationRepository Repository;
        public IpInformationTests(MongoDbContainerFixture mongoDbContainerFixture)
        {
            MongoDbContainerFixture _mongoDbContainerFixture = mongoDbContainerFixture;
            MongoDbConnector _mongoDbConnector = new(_mongoDbContainerFixture.ConnectionString, "test");
            Repository = new(_mongoDbConnector);
        }


        [Fact]
        public async Task CreateIp_ShouldSuccess()
        {
            // 创建 IpInformation 实例
            var ipInfo = new IpInformation
            {
                IpStart = 123,
                IpEnd = 345,
                RegionName = "liwan",
                CountryName = "china",
                CityName = "gz",
            };
            await Repository.CreateAsync(ipInfo);
            var result = await Repository.IpInformation.Find(x => x.IpStart == 123).FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal(ipInfo, result);
        }

        [Fact]
        public async Task LookupIp_WhenValidIp_ShouldReturnIpInformation()
        {
            var ipInfo = new IpInformation
            {
                IpStart = 12345,
                IpEnd = 34567,
                RegionName = "liwan1",
                CountryName = "china",
                CityName = "gz1",
            };
            await Repository.CreateAsync(ipInfo);
            IpInformation? IpInfo = await Repository.GetByIpAsync(12345);
            Assert.NotNull(IpInfo);
            Assert.Equal("gz1", IpInfo.CityName);
        }

        [Fact]
        public async Task LookupIp_WhenNoRecord_ShouldReturnNull()
        {
            IpInformation? IpInfo = await Repository.GetByIpAsync(1000);
            Assert.Null(IpInfo);
            //await Assert.ThrowsAsync<InvalidOperationException>(() => Repository.GetByIpAsync(1000));
        }
    }
}
