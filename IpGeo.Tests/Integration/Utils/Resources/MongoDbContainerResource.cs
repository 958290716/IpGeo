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
            .WithExtraHost("host.docker.internal", "host-gateway")
            .WithCommand("--replSet", "repro")
            .Build();
        await container.StartAsync();
        await container.ExecScriptAsync(
            $"rs.initiate({{_id:'repro',members:[{{_id:0,host:'host.docker.internal:{container.GetMappedPublicPort(27017)}'}}]}})"
        );
        ConnectionString = container.GetConnectionString();
    }

    public Task DisposeAsync()
    {
        // Fire and forget (no await)
        container?.StopAsync();
        return Task.CompletedTask;
    }
}
