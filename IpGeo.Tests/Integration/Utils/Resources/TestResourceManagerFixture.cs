using Microsoft.Extensions.DependencyInjection;

namespace IpGeo.Tests.Integration.Utils.Resources;

[CollectionDefinition(nameof(TestResourceManagerFixture))]
public class TestAppCreatorCollection : ICollectionFixture<TestResourceManagerFixture> { }

public sealed class TestResourceManagerFixture : ITestResourceManager, IAsyncLifetime
{
    private ServiceProvider? testResourceProvider;

    private ServiceProvider TestResourceProvider
    {
        get
        {
            return testResourceProvider
                ?? throw new InvalidOperationException(
                    $"{nameof(IAsyncLifetime)}.{nameof(InitializeAsync)} must be called before accessing test resources. {nameof(TestResourceManagerFixture)} should only be used in xUnit test fixtures."
                );
        }
        set { testResourceProvider = value; }
    }

    /// <summary>
    /// All test resources must be registered here.
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureResources(IServiceCollection services)
    {
        services.AddResource<MongoDbContainerResource>();
    }

    public async Task<T> GetResource<T>()
        where T : ITestResource, new()
    {
        return await TestResourceProvider.GetResource<T>();
    }

    public Task InitializeAsync()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureResources(serviceCollection);

        TestResourceProvider = serviceCollection.BuildServiceProvider();

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await TestResourceProvider.DisposeAsync();
    }
}

public static class TestResourceInjectionExtensions
{
    /// <summary>
    /// Holds a lazily created resource.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class LazyAsyncResource<T> : IAsyncDisposable
        where T : ITestResource, new()
    {
        private readonly Lazy<Task<T>> resource;

        public Task<T> Resource => resource.Value;

        public LazyAsyncResource() => resource = new Lazy<Task<T>>(CreateAsync);

        private static async Task<T> CreateAsync()
        {
            var resource = new T();
            await resource.InitializeAsync();
            return resource;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (resource.IsValueCreated)
            {
                var a = await resource.Value;
                await a.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    public static IServiceCollection AddResource<T>(this IServiceCollection services)
        where T : ITestResource, new()
    {
        services.AddSingleton<LazyAsyncResource<T>>();
        return services;
    }

    public static async Task<T> GetResource<T>(this ServiceProvider servicesProvider)
        where T : ITestResource, new()
    {
        return await servicesProvider.GetRequiredService<LazyAsyncResource<T>>().Resource;
    }
}
