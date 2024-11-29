using IpGeo.IpLookup.Data;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    public class MongoIpInformationRepositoryTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        MongoIpInformationRepository Repository => throw new NotImplementedException();

        [Fact]
        public async Task CreateIp_ShouldSuccess()
        {
            await Repository.CreateAsync(null!);
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(0)]
        public Task LookupIp_WhenValidIp_ShouldReturnIpInformation(uint ip) =>
            throw new NotImplementedException();

        [Theory]
        [InlineData(0)]
        public Task LookupIp_WhenNoRecord_ShouldReturnNull(uint ip) =>
            throw new NotImplementedException();
    }
}
