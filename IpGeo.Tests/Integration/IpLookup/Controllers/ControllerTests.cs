using System.Diagnostics;
using System.Text;
using IpGeo.Dto;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using IpGeo.Tests.Integration.Utils;
using IpGeo.Tests.Integration.Utils.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IpGeo.Tests.Integration.IpLookup.Controllers
{
    [Collection(nameof(TestResourceManagerFixture))]
    public class ControllerTests(
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
            var ipInfo = IpInfoSetUp(ipStart: 123, ipEnd: 345);
            await using var scope = _factory.Services.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<IIpInformationRepository>();
            await repository.CreateAsync(ipInfo);
            var client = _factory.CreateClient();
            var getIpInfo = await client.GetAsync("/api/IpGeoController/123"); //+"23456"
            Debug.WriteLine(await getIpInfo.Content.ReadAsStringAsync());
            getIpInfo.EnsureSuccessStatusCode();
            var content = await getIpInfo.Content.ReadAsStringAsync();
            Assert.Contains("123", content);
        }

        private static PostIpInfoRequest SeedPostIpInfoRequest(
            string ipStart = "0",
            string ipEnd = "12",
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

        private static IpInformation IpInfoSetUp(
            uint ipStart = 12,
            uint ipEnd = 34,
            string? regionName = null,
            string? countryName = null,
            string? cityName = null
        )
        {
            return new IpInformation
            {
                RegionName = regionName ?? Guid.NewGuid().ToString(),
                CityName = cityName ?? Guid.NewGuid().ToString(),
                CountryName = countryName ?? Guid.NewGuid().ToString(),
                IpStart = ipStart,
                IpEnd = ipEnd,
            };
        }
    }
}
