using System.Globalization;
using System.IO.Compression;
using System.Net;
using CsvHelper;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using MongoDB.Driver;

namespace IpGeo.Services
{
    public class CsvService(IpLookupMongoDbContext ipLookupMongoDbContext) : ICsvService
    {
        private readonly IMongoCollection<IpInformation> _ipInformation =
            ipLookupMongoDbContext.IpLocationCollection;

        public async Task DownloadAndSaveCsvDataAsync(string csvUrl)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(csvUrl);
            response.EnsureSuccessStatusCode();

            var gzContent = await response.Content.ReadAsStreamAsync();
            using var gzipStream = new GZipStream(gzContent, CompressionMode.Decompress);
            using var reader = new StreamReader(gzipStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CsvData>();
            int i = 0;
            foreach (var record in records)
            {
                if (i > 100)
                    break;
                var decimalIpStart = IpToDecimal(record.IpStart);
                var decimalIpEnd = IpToDecimal(record.IpEnd);
                var ipInfo = new IpInformation
                {
                    IpStart = decimalIpStart,
                    IpEnd = decimalIpEnd,
                    CityName = record.CityName,
                    RegionName = record.RegionName,
                    CountryName = record.CountryName,
                };
                await _ipInformation.InsertOneAsync(ipInfo);
                i++;
            }
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
