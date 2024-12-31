namespace IpGeo.Dto
{
    public record PostIpInfoRequest
    {
        public required uint IpStart { get; init; }
        public required uint IpEnd { get; init; }
        public required string RegionName { get; init; }
        public required string CountryName { get; init; }
        public required string CityName { get; init; }
    }
}
