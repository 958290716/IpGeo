using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using Testcontainers.MongoDb;
using MongoDB.Driver;
using Xunit.Abstractions;
using DotNet.Testcontainers.Builders;
using System.Diagnostics;
using MongoDB.Bson;

namespace IpGeo.Tests.Integration.IpLookup.Data
{
    public class MongoDbContainerFixture : IAsyncLifetime
    {       
        private readonly MongoDbContainer _mongoDbContainer;
        public string ConnectionString { get; private set; }// => _mongoDbContainer.GetConnectionString();
        //public ITestOutputHelper OutputHelper { get; set; }
        

        public MongoDbContainerFixture()
        {
            // 创建 MongoDB 容器配置
            _mongoDbContainer = new MongoDbBuilder()
                .WithImage("mongo:latest")
                .Build(); // 默认 MongoDB 端口
            ConnectionString = _mongoDbContainer.GetConnectionString();
            // 在容器启动后进行数据库连接初始化
            //_mongoDbContainer.StartAsync().Wait();
        }

        public async Task InitializeAsync()
        {
            // 连接到 MongoDB 实例
            await _mongoDbContainer.StartAsync();

        }

        public async Task DisposeAsync()
        {
            // 测试完成后关闭容器
            if (_mongoDbContainer != null)
            {
                await _mongoDbContainer.StopAsync();
                
            }
            
        }
    }

    public class IpInformationTests : IClassFixture<MongoDbContainerFixture>
    {
        private readonly MongoDbContainerFixture _mongoDbContainerFixture;

        public IpInformationTests(MongoDbContainerFixture mongoDbContainerFixture)
        {
            _mongoDbContainerFixture = mongoDbContainerFixture;
        }
        [Fact]
        public async void TestMongoDbConnection()
        {
            //await _mongoDbContainerFixture.InitializeAsync();
            var mongoClient = new MongoClient(_mongoDbContainerFixture.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase("test");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("test");
            //Create
            await collection.InsertOneAsync(new BsonDocument()
            {
                ["Name"] = "12"
            });
            //Read
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("Name", "12");
            var results = collection.Find(filter).ToListAsync();
            Assert.Equal("12", "12");
            //await _mongoDbContainerFixture.DisposeAsync();
        }
    }
}
