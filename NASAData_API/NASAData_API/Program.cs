using NASAData_API.MappingProfiles;
using NASAData_API.Repositories;
using NASAData_API.HostedServices;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string _dbConnectionString = builder.Configuration.GetConnectionString("NasaDB");


builder.Services.AddControllers(options =>
{
    options.Filters.Add<SampleExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMeteoriteRepository>(isp => new MeteoriteRepository(_dbConnectionString));
builder.Services.AddAutoMapper(typeof(MeteoriteMappingProfile));
builder.Services.AddHostedService<MeteoritesDataWatcherService>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();
