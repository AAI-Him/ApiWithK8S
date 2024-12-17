using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions.ZFI_RFC_PARTIAL_PAYMENT
{
    internal class GetRFCPartialPaymentOutputParameter : BaseSAPOutputParameters
    {
        [RfcEntityProperty("OUT_TAB")]
        public GetRFCPartialPaymentOutputTable[]? outTable { get; set; }
    }

    public sealed class GetRFCPartialPaymentOutputTable
    {
        [RfcEntityProperty("MANDT")]
        public string? ClientNo { get; set; }
        [RfcEntityProperty("LIFNR")]
        public string? VendorCode { get; set; }
        [RfcEntityProperty("NAME")]
        public string? VendorName { get; set; }
        [RfcEntityProperty("BELNR")]
        public string? MMInvoiceNo { get; set; }
        [RfcEntityProperty("GJAHR")]
        public int InvoiceDocumentYear { get; set; }
        [RfcEntityProperty("BURKS")]
        public string? CompanyCode { get; set; }
        [RfcEntityProperty("AUGBL")]
        public string? ClearingDocumentNo { get; set; }
        [RfcEntityProperty("AUGGJ")]
        public int ClearingDocumentYear { get; set; }
        [RfcEntityProperty("WAERS")]
        public string? Currency {  get; set; }
        [RfcEntityProperty("WRBTR")]
        public Decimal PaymentAmountDocumentCurrency { get; set; }
        [RfcEntityProperty("DMBTR")]
        public Decimal PaymentAmountHKD { get; set; }
    }
}
