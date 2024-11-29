namespace IpGeo.api
{
    public interface IRepository
    {
        Task<List<int>> GetAllAsync();
    }
}
