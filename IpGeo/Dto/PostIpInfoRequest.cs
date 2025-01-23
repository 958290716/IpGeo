namespace IpGeo.Dto
{
    public record PostIpInfoRequest
    {
        public required string IpStart { get; init; }
        public required string IpEnd { get; init; }
        public required string RegionName { get; init; }
        public required string CountryName { get; init; }
        public required string CityName { get; init; }
    }
}
