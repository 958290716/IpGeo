namespace IpGeo.Tests.Integration.Utils;

public abstract class SimpleTestSetup<TSystemUnderTest> : IAsyncLifetime
    where TSystemUnderTest : class
{
    public TSystemUnderTest Sut { get; private set; } = null!;

    public virtual async Task InitializeAsync() => Sut = await GetSutAsync();

    public virtual Task DisposeAsync() => Task.CompletedTask;

    protected abstract Task<TSystemUnderTest> GetSutAsync();
}
