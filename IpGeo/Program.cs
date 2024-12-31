using IpGeo.Controllers;
using IpGeo.IpLookup.Data;
using IpGeo.IpLookup.Models;
using IpGeo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IIpInformationRepository, MongoIpInformationRepository>();
builder.Services.AddScoped<IpLookupMongoDbContext>();
builder.Services.Configure<IpLookupMongoDbContextSettings>(builder.Configuration.GetSection("IpLookupMongoDbContextSettings"));
//builder.Services.Configure<IpLookupMongoDbContextSettings>(
//    builder.Configuration.GetSection("IpLookupMongoDbContextSettings")
//);
//builder.Services.Configure<IpLookupMongoDbContextSettings>(
//    builder.Configuration.GetSection("IpLookupMongoDbContextSettings")
//);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }

//public class Startup
//{
//    private readonly IConfiguration _configuration;

//    public Startup(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
//    {
//        var builder;
//        _configuration = builder.Build();
//    }

//    public void ConfigureServices(IServiceCollection services)
//    {
//        services.AddTransient<ICsvService, CsvService>();
//        services.Configure<IpLookupMongoDbContext>(Configuration.GetSection("Mongo"));
//    }
//}
