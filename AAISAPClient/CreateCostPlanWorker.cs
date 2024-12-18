using AAISAPClient.SapRfcFunctions;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SapCo2.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient
{
    internal class CreateCostPlanWorker(
        ILogger<CreateCostPlanWorker> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        IAmazonSQS sqsClient)
        : BaseSQSWorker(logger, serviceProvider, sqsClient)
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _AWSSQSChannelOption = configuration.GetSection("SAPService").Get<Dictionary<string, AWSSQSChannelOption>>()?[AAISAPConstants.ZRFC135_CREATE_COSTPLAN_V4];
            Console.WriteLine(JsonConvert.SerializeObject(_AWSSQSChannelOption));
            await base.ExecuteAsync(stoppingToken);
        }
    }
}
