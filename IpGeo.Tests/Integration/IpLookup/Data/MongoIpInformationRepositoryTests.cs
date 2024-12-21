using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using IpGeo.Services;
using IpGeo.Tests.Integration.Utils;
using IpGeo.Tests.Integration.Utils.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    [Collection(nameof(TestResourceManagerFixture))]
    public class MongoIpInformationRepositoryTests(
        TestResourceManagerFixture manager,
        WebApplicationFactory<Program> factory
    ) : SimpleTestSetup<MongoIpInformationRepository>, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        protected override async Task<MongoIpInformationRepository> GetSutAsync()
        {
            // Get the mongodb test container.
            var mongodb = await manager.GetResource<MongoDbContainerResource>();

            // Setup your repository here.
            var connectionString = mongodb.ConnectionString;
            IpLookupMongoDbContextSettings ipLookupMongoDbContextSettings = new(
                connectionString,
                "test",
                "IpLocation"
            );
            IpLookupMongoDbContext ipLookupMongoDbContext = new(ipLookupMongoDbContextSettings);
            MongoIpInformationRepository mongoIpInformationRepository = new(ipLookupMongoDbContext);
            return mongoIpInformationRepository;
        }

        //[Theory]
        //[InlineData("/")]
        //[InlineData("/Index")]
        //[InlineData("/About")]
        //[InlineData("/Privacy")]
        //[InlineData("/Contact")]
        //public async Task GetDataAndStoreDataAsync(string url)
        //{
        //    // Arrange
        //    var client = _factory.CreateClient();

        //    // Act
        //    var response = await client.GetAsync(url);

        //    // Assert
        //    response.EnsureSuccessStatusCode(); // Status Code 200-299
        //    Assert.NotNull(response.Content.Headers.ContentType);
        //    Assert.Equal(
        //        "text/html; charset=utf-8",
        //        response.Content.Headers.ContentType.ToString()
        //    );
        //}

        [Fact]
        public async Task CreateUser_returnsCreatedStatusCode()
        {
            var ipInfo = SeedIpInformation(ipStart: 12345, ipEnd: 34567);
            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(ipInfo),
                Encoding.UTF8,
                "application/json"
            );
            var client = _factory.CreateClient();
            var response = await client.PostAsync("/api/GetAndSetDataIntoDatabase/", jsonContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("12345", content);
        }

        //[Fact]
        //public async Task CreateIp_ShouldSuccess()
        //{
        //    /// Sut is your repository configured in <see cref="GetSutAsync"/>.
        //    var ipInfo = SeedIpInformation(ipStart: 123, ipEnd: 345);
        //    await Sut.CreateAsync(ipInfo);
        //    var result = await Sut.IpInformation.Find(x => x.IpStart == 123).FirstOrDefaultAsync();

        //    Assert.NotNull(result);
        //    Assert.Equal(ipInfo, result);
        //}

        //[Fact]
        //public async Task LookupIp_WhenValidIp_ShouldReturnIpInformation()
        //{
        //    var ipInfo = SeedIpInformation(ipStart: 12345, ipEnd: 34567);
        //    await Sut.CreateAsync(ipInfo);
        //    IpInformation? IpInfo = await Sut.GetByIpAsync(12345);
        //    Assert.NotNull(IpInfo);
        //    Assert.Equal(ipInfo.CityName, IpInfo.CityName);
        //}

        //[Fact]
        //public async Task LookupIp_WhenNoRecord_ShouldReturnNull()
        //{
        //    IpInformation? IpInfo = await Sut.GetByIpAsync(1000);
        //    Assert.Null(IpInfo);
        //}

        public static IpInformation SeedIpInformation(
            uint ipStart = 0,
            uint ipEnd = 1,
            string? regionName = null,
            string? countryName = null,
            string? cityName = null
        )
        {
            return new IpInformation
            {
                IpStart = ipStart,
                IpEnd = ipEnd,
                RegionName = regionName ?? Guid.NewGuid().ToString(),
                CityName = cityName ?? Guid.NewGuid().ToString(),
                CountryName = countryName ?? Guid.NewGuid().ToString(),
            };
        }
    }
}
