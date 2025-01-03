namespace IpGeo.Services
{
    public interface ICsvService
    {
        public Task DownloadAndSaveCsvDataAsync(string csvUrl);
        public uint IpToDecimal(string ip);
    }
}
