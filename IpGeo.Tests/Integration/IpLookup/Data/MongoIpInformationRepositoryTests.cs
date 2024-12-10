using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using IpGeo.Tests.Integration.Utils;
using IpGeo.Tests.Integration.Utils.Resources;
using MongoDB.Driver;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    [Collection(nameof(TestResourceManagerFixture))]
    public class MongoIpInformationRepositoryTests(TestResourceManagerFixture manager)
        : SimpleTestSetup<MongoIpInformationRepository>
    {
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

        [Fact]
        public async Task CreateIp_ShouldSuccess()
        {
            /// Sut is your repository configured in <see cref="GetSutAsync"/>.
            var ipInfo = SeedIpInformation(ipStart: 123, ipEnd: 345);
            await Sut.CreateAsync(ipInfo);
            var result = await Sut.IpInformation.Find(x => x.IpStart == 123).FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal(ipInfo, result);
        }

        [Fact]
        public async Task LookupIp_WhenValidIp_ShouldReturnIpInformation()
        {
            var ipInfo = SeedIpInformation(ipStart: 12345, ipEnd: 34567);
            await Sut.CreateAsync(ipInfo);
            IpInformation? IpInfo = await Sut.GetByIpAsync(12345);
            Assert.NotNull(IpInfo);
            Assert.Equal(ipInfo.CityName, IpInfo.CityName);
        }

        [Fact]
        public async Task LookupIp_WhenNoRecord_ShouldReturnNull()
        {
            IpInformation? IpInfo = await Sut.GetByIpAsync(1000);
            Assert.Null(IpInfo);
        }

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
