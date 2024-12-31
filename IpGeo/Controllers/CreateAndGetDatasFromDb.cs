using IpGeo.Api;
using IpGeo.Dto;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace IpGeo.Controllers
{
    [ApiController]
    [Route("api/GetAndSetDataIntoDatabase")]
    public class CreateAndGetDataFromDb(IIpInformationRepository mongoIpInformationRepository)
        : ControllerBase
    {
        private readonly IIpInformationRepository _mongoIpInformationRepository =
            mongoIpInformationRepository;


        [HttpGet("{ip}")]
        public ActionResult<List<IpInformation>> GetDataByIp(uint ip)
        {
            var ipInfo = _mongoIpInformationRepository.GetByIpAsync(ip);
            if (ipInfo == null)
            {
                return NotFound("Data not found");
            }           
            return Ok(ipInfo);
        }

        [HttpPost]
        public ActionResult<IpInformation> AddIpInfo([FromBody] PostIpInfoRequest postIpInfoRequest)
        {
            var ipInfo = new IpInformation() { 
                CityName = postIpInfoRequest.CityName,
                CountryName = postIpInfoRequest.CountryName,
                IpEnd = postIpInfoRequest.IpEnd,
                IpStart = postIpInfoRequest.IpStart,
                RegionName = postIpInfoRequest.RegionName,
            };


            var newIpInfo = _mongoIpInformationRepository.CreateAsync(ipInfo);
            return CreatedAtAction(
                nameof(GetDataByIp),
                new { ip = postIpInfoRequest.IpStart },
                postIpInfoRequest
            );
        }
    }
}
