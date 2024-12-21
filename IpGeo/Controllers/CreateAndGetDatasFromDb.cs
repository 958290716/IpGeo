using IpGeo.Api;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace IpGeo.Controllers
{
    [ApiController]
    [Route("api/GetAndSetDataIntoDatabase")]
    public class CreateAndGetDatasFromDb(MongoIpInformationRepository mongoIpInformationRepository)
        : ControllerBase
    {
        private readonly MongoIpInformationRepository _mongoIpInformationRepository =
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
        public ActionResult<IpInformation> AddIpInfo([FromBody] IpInformation ipInformation)
        {
            var newIpInfo = _mongoIpInformationRepository.CreateAsync(ipInformation);
            return CreatedAtAction(
                nameof(GetDataByIp),
                new { ip = ipInformation.IpStart },
                ipInformation
            );
        }
    }
}
