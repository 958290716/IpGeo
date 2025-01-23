using System.Security.Authentication;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IpGeo.Services
{
    public class RepositoryService(IIpInformationRepository mongoIpInformationRepository)
        : IRepositoryService
    {
        private readonly IIpInformationRepository _mongoIpInformationRepository =
            mongoIpInformationRepository;

        public async Task<IpInformation> GetDataByIpService(uint ipStart)
        {
            var data = await _mongoIpInformationRepository.GetByIpAsync(ipStart);
            if (data != null)
            {
                return data;
            }
            throw new Exception();
        }

        public async Task CreateService(IpInformation ipInformation)
        {
            await _mongoIpInformationRepository.CreateAsync(ipInformation);
        }
    }
}
