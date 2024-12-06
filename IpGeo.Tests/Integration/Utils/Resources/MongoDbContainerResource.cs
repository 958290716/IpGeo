using Testcontainers.MongoDb;

namespace IpGeo.Tests.Integration.Utils.Resources;

public class MongoDbContainerResource : ITestResource
{
    public string ConnectionString { get; private set; } = null!;
    private MongoDbContainer? container;

    public async Task InitializeAsync()
    {
        container = new MongoDbBuilder()
            .WithUsername(null)
            .WithPassword(null)
            .WithCommand("--replSet", "repro")
            .Build();
        await container.StartAsync();
        await container.ExecAsync(["mongosh", "--quiet", "--eval", "\"rs.initiate();\""]);
        ConnectionString = container.GetConnectionString();
    }

    public Task DisposeAsync()
    {
        // Fire and forget (no await)
        container?.StopAsync();
        return Task.CompletedTask;
    }
}
