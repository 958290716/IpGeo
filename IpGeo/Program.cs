using IpGeo.IpLookup.Data;
using IpGeo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IIpInformationRepository, MongoIpInformationRepository>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IpLookupMongoDbContext>();
builder.Services.AddSingleton(new HttpClient());
builder.Services.Configure<IpLookupMongoDbContextSettings>(
    builder.Configuration.GetSection("IpLookupMongoDbContextSettings")
);

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
