using System.Net;
using IpGeo.Dto;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using IpGeo.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpGeo.Controllers
{
    [ApiController]
    [Route("api/IpGeoController")]
    public class CreateAndGetDataFromDb(IRepositoryService repositoryService) : ControllerBase
    {
        private readonly IRepositoryService _repositoryService = repositoryService;

        [HttpGet("{ip}")]
        public async Task<ActionResult<List<IpInformation>>> GetDataByIp(string ip)
        {
            var ip1 = IpToDecimal(ip);
            if (_repositoryService == null)
            {
                throw new Exception("_repositoryService is null");
            }
            var ipInfo = await _repositoryService.GetDataByIpService(ip1);
            if (ipInfo == null)
            {
                return NotFound("Data not found");
            }
            return Ok(ipInfo);
        }

        public uint IpToDecimal(string ip)
        {
            // 使用 IPAddress.Parse 将 IP 地址解析为一个 IPAddress 对象
            var ipAddress = IPAddress.Parse(ip);

            // 获取字节数组
            byte[] bytes = ipAddress.GetAddressBytes();

            // 将字节数组转换为十进制整数
            uint decimalValue = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                decimalValue |= (uint)bytes[i] << (8 * (3 - i));
            }

            return decimalValue;
        }
    }
}
