using AAISAPClient;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Core.Extensions;
using SapCo2.Extensions;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.TryAddScoped<>();
builder.Services.AddSapCo2(s => s.ReadFromConfiguration(builder.Configuration));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
