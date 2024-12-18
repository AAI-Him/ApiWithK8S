using AAISAPClient.SapRfcFunctions;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SapCo2.Abstract;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace AAISAPClient
{
    internal class GetRFCPartialPaymentWorker(
        ILogger<GetRFCPartialPaymentWorker> logger, 
        IServiceProvider serviceProvider, 
        IConfiguration configuration,
        IAmazonSQS sqsClient) 
        : BaseSQSWorker(logger, serviceProvider, sqsClient)
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _AWSSQSChannelOption = configuration.GetSection("SAPService").Get<Dictionary<string, AWSSQSChannelOption>>()?[AAISAPConstants.ZFI_RFC_PARTIAL_PAYMENT];

            Console.WriteLine(JsonConvert.SerializeObject(_AWSSQSChannelOption));

            await base.ExecuteAsync(stoppingToken);
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<string>(message.Body);

                if (!DateTime.TryParse(payload, out DateTime cutoffdate))
                {
                    throw new ArgumentException("Invalid argument cutoffdate.");
                }

                // We are not supposed to return full RFCPartialPayment report here
                //using (IAAISAPServiceClient sapServiceClient = serviceProvider.GetRequiredService<IAAISAPServiceClient>())
                //{
                //    await sapServiceClient.GetRFCPartialPaymentAsync(cutoffdate);
                //}
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
