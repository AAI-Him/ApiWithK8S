using AAISAPClient.SapRfcFunctions.CREATE_COSTPLAN_SQL_V4;
using AAISAPClient.SapRfcFunctions.ZFI_RFC_PARTIAL_PAYMENT;
using SapCo2.Abstract;

namespace AAISAPClient
{
    public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) 
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                //var inputParameters = new CreateCostPlanSQLV4InputParameter
                //{
                //    ProjectId = "",
                //    Description = "",
                //    SAPKey = "",
                //    SAPUserId = ""
                //};
                var inputParameters = new GetRFCPartialPaymentInputParameter() {
                    AccountGroup = new GetRFCPartialPaymentInputTable() {
                        AccountType = "Z21"
                    },
                    CutOffDate = "",
                    SAPUserId = "",
                    SAPKey = "",
                };
                using (IRfcClient client = serviceProvider.GetRequiredService<IRfcClient>())
                {
                    //var result = await client.ExecuteRfcAsync<CreateCostPlanSQLV4InputParameter, CreateCostPlanSQLV4OutputParameter>("CS_BOM_EXPL_MAT_V2_RFC", inputParameters);
                    var result = await client.ExecuteRfcAsync<GetRFCPartialPaymentInputParameter, GetRFCPartialPaymentOutputParameter>("ZFI_RFC_PARTIAL_PAYMENT", inputParameters);
                }
                //BomOutputParameter result = null;

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
