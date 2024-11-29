namespace IpGeo.api.Models
{
    public class IpGeoData
    {
        public int ipStart {  get; set; }
        public int ipEnd { get; set; }

        public string regionName {  get; set; } = string.Empty;
        public string countryName {  get; set; } = string.Empty;
        public string cityName { get; set; } = string.Empty;
    }
}
