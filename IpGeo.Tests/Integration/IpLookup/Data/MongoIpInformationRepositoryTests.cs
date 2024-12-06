using IpGeo.IpLookup.Data;
using IpGeo.Tests.Integration.Utils;
using IpGeo.Tests.Integration.Utils.Resources;

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

            throw new NotImplementedException();
        }

        [Fact]
        public async Task CreateIp_ShouldSuccess()
        {
            /// Sut is your repository configured in <see cref="GetSutAsync"/>.
            await Sut.CreateAsync(null!);
            throw new NotImplementedException();
        }

        [Fact]
        public Task LookupIp_WhenValidIp_ShouldReturnIpInformation()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public Task LookupIp_WhenNoRecord_ShouldReturnNull()
        {
            throw new NotImplementedException();
        }
    }
}
