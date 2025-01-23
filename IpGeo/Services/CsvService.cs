using System.Globalization;
using System.IO.Compression;
using System.Net;
using CsvHelper;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IpGeo.Services
{
    public class CsvService(
        IIpInformationRepository mongoIpInformationRepository,
        HttpClient client
    ) : ICsvService
    {
        private readonly IIpInformationRepository _mongoIpInformationRepository =
            mongoIpInformationRepository;
        private readonly HttpClient _httpClient = client;

        private static uint IpToDecimal(string ip)
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

        public async Task DownloadAndSaveCsvDataAsync(string csvUrl)
        {
            var response = await _httpClient.GetAsync(csvUrl);
            response.EnsureSuccessStatusCode();
            var gzContent = await response.Content.ReadAsStreamAsync();
            using var gZipStream = new GZipStream(gzContent, CompressionMode.Decompress);
            using var reader = new StreamReader(gZipStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CsvData>();
            //var record = records.FirstOrDefault() ?? throw new Exception("record is null");
            var ConvertData = ReadCsvAndConvert(records);
            await _mongoIpInformationRepository.CreateManyAsync(ConvertData);
        }

        static List<IpInformation> ReadCsvAndConvert(IEnumerable<CsvData> records)
        {
            var documents = new List<IpInformation>();
            foreach (var record in records)
            {
                var intIpStart = IpToDecimal(record.IpStart);
                var intIpEnd = IpToDecimal(record.IpEnd);
                var doc = new IpInformation
                {
                    IpStart = intIpStart,
                    IpEnd = intIpEnd,
                    RegionName = record.RegionName,
                    CityName = record.CityName,
                    CountryName = record.CountryName,
                };
                documents.Add(doc);
            }

            return documents;
        }
    }
}
