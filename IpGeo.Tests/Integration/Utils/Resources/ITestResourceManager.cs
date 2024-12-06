namespace IpGeo.Tests.Integration.Utils.Resources;

public interface ITestResourceManager
{
    public Task<T> GetResource<T>()
        where T : ITestResource, new();
}
