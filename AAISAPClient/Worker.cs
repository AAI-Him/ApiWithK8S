using AAISAPClient.SapRfcFunctions;
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

                try
                {
                    using (IRfcClient client = serviceProvider.GetRequiredService<IRfcClient>())
                    {
                        #region ZFI_RFC_PARTIAL_PAYMENT
                        //var inputParameters = new GetRFCPartialPaymentInputParameter()
                        //{
                        //    SKtokk = new GetRFCPartialPaymentInputTable[] {
                        //        new GetRFCPartialPaymentInputTable{
                        //            SIGN = "I",
                        //            OPTION = "EQ",
                        //            LOW = "20240927",
                        //            HIGH = ""
                        //        }
                        //    },
                        //    CutOffDate = "20240927",
                        //};

                        //var result = await client.ExecuteRfcAsync<GetRFCPartialPaymentInputParameter, GetRFCPartialPaymentOutputParameter>(AAISAPConstants.ZFI_RFC_PARTIAL_PAYMENT, inputParameters);

                        //foreach (var item in result.outTable)
                        //{
                        //    Console.WriteLine(string.Join(",", item.ClientNo, item.VendorCode, item.VendorName, item.MMInvoiceNo, item.MMInvoiceNo, item.InvoiceDocumentYear, item.CompanyCode, item.ClearingDocumentNo, item.ClearingDocumentYear, item.Currency, item.PaymentAmountDocumentCurrency, item.PaymentAmountHKD));
                        //}
                        //Console.WriteLine($"ZFI_RFC_PARTIAL_PAYMENT executed, retrieved record: {result.outTable.Length}");
                        #endregion

                        #region CREATE_COSTPLAN_SQL_V4

                        var costCodes = new List<CreateCostPlanSQLV4InputTable>();
                        for (int i = 0; i < 1; i++)
                        {
                            costCodes.Add(new CreateCostPlanSQLV4InputTable
                            {
                                //Client = "700",
                                ProjectId = "1095",
                                CompanyCode = "D007",
                                CostCenter = "PROJ1095",
                                ProjectStartDate = "20231115",
                                ProjectEndDate = "20240528",
                                CostCode = "0",
                                CodeDescription = "Area 0 (m)",
                                MFRItemNo = "",
                            });
                        }

                        var inputParameters = new CreateCostPlanSQLV4InputParameter()
                        {
                            //Client = "700",
                            ProjectId = "1095",
                            Description = "Fitting-Out Works for 2/F Medical Center at 21 Ashley Road".Substring(0, 40),
                            InTable = costCodes.ToArray()
                        };

                        var result = await client.ExecuteRfcAsync<CreateCostPlanSQLV4InputParameter, CreateCostPlanSQLV4OutputParameter>(AAISAPConstants.ZRFC135_CREATE_COSTPLAN_V4, inputParameters);
                        Console.WriteLine($"ZRFC135_CREATE_COSTPLAN_V4 executed, retrieved record: {result?.outTable?.Length}");
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    await Task.Delay(50000, stoppingToken);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
