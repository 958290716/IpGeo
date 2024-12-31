using System.Diagnostics;
using System.Text;
using IpGeo.Dto;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using IpGeo.Tests.Integration.Utils;
using IpGeo.Tests.Integration.Utils.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    [Collection(nameof(TestResourceManagerFixture))]
    public class ControllerTests(
        TestResourceManagerFixture manager,
        WebApplicationFactory<Program> factory
    ) : SimpleTestSetup<MongoIpInformationRepository>, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        //protected override async Task<MongoIpInformationRepository> GetSutAsync()
        //{
        //    // Get the mongodb test container.
        //    var mongodb = await manager.GetResource<MongoDbContainerResource>();

        //    // Setup your repository here.
        //    //IpLookupMongoDbContextSettings ipLookupMongoDbContextSettings = new();
        //    var ipLookupMongoDbContextSettings = new IpLookupMongoDbContextSettings();

        //    IpLookupMongoDbContext ipLookupMongoDbContext = new(Options.Create(ipLookupMongoDbContextSettings));
        //    MongoIpInformationRepository mongoIpInformationRepository = new(ipLookupMongoDbContext);
        //    return mongoIpInformationRepository;
        //}

        protected override async Task<MongoIpInformationRepository> GetSutAsync()
        {
            // Get the mongodb test container.
            var mongodb = await manager.GetResource<MongoDbContainerResource>();

            // Setup your repository here.
            var connectionString = mongodb.ConnectionString;
            IpLookupMongoDbContextSettings ipLookupMongoDbContextSettings = new()
            {
                CollectionName = "IpLocation",
                DatabaseName = "test",
                ConnectString = connectionString,
            };

            IpLookupMongoDbContext ipLookupMongoDbContext = new(
                Options.Create(ipLookupMongoDbContextSettings)
            );
            MongoIpInformationRepository mongoIpInformationRepository = new(ipLookupMongoDbContext);
            return mongoIpInformationRepository;
        }

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
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("12345", content);
        }

        public static PostIpInfoRequest SeedIpInformation(
            uint ipStart = 0,
            uint ipEnd = 1,
            string? regionName = null,
            string? countryName = null,
            string? cityName = null
        )
        {
            return new PostIpInfoRequest
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
