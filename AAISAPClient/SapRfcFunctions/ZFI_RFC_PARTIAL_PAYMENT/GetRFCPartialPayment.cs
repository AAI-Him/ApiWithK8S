using SapCo2.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions
{
    public class GetRFCPartialPayment(IRfcClient client)
    {
        public async Task<List<GetRFCPartialPaymentOutputTable>?> ExecuteAsync(DateTime cutoffDate)
        {
            var inputParameters = new GetRFCPartialPaymentInputParameter()
            {
                SKtokk = new GetRFCPartialPaymentInputTable[] {
                    new GetRFCPartialPaymentInputTable{
                        SIGN = "I",
                        OPTION = "EQ",
                        LOW = cutoffDate.ToString("yyyyMMdd"),
                        //LOW = "20240927",
                        HIGH = ""
                    }
                },
                CutOffDate = cutoffDate.ToString("yyyyMMdd"),
            };

            var result = await client.ExecuteRfcAsync<GetRFCPartialPaymentInputParameter, GetRFCPartialPaymentOutputParameter>(AAISAPConstants.ZFI_RFC_PARTIAL_PAYMENT, inputParameters);

            foreach (var item in result?.outTable)
            {
                Console.WriteLine(string.Join(",", item.ClientNo, item.VendorCode, item.VendorName, item.MMInvoiceNo, item.MMInvoiceNo, item.InvoiceDocumentYear, item.CompanyCode, item.ClearingDocumentNo, item.ClearingDocumentYear, item.Currency, item.PaymentAmountDocumentCurrency, item.PaymentAmountHKD));
            }
            Console.WriteLine($"ZFI_RFC_PARTIAL_PAYMENT executed, retrieved record: {result.outTable.Length}");
            return result?.outTable?.ToList();
        }
    }
}
