using ApiWithK8S.Plugins;

Console.WriteLine($"ASPNETCORE_ENVIRONMENT:{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

var config = new ConfigMapToAppSettings(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development");
config.GenerateAppSettingsAsync().Wait();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCommandLine();
// Add services to the container.

builder.Services.AddControllers();
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