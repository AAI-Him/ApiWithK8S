using AAISAPClient;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SapCo2.Core.Extensions;
using SapCo2.Extensions;

Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", "test");
Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", "test");
Environment.SetEnvironmentVariable("AWS_ENDPOINT_URL", "http://localstack.localstack.svc.cluster.local:4566");

var sqsConfig = new AmazonSQSConfig
{
    ServiceURL = "http://localstack.localstack.svc.cluster.local:4566"
};

var credential = new AmazonSQSClient("test", "test", "token", sqsConfig);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddSapCo2(s => s.ReadFromConfiguration(builder.Configuration));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
