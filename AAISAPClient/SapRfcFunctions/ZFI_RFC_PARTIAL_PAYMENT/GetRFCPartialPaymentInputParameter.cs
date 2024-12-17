using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions.ZFI_RFC_PARTIAL_PAYMENT
{
    internal class GetRFCPartialPaymentInputParameter : IBaseSAPInputParameters
    {
        [RfcEntityProperty("S_KTOKK")]
        public GetRFCPartialPaymentInputTable[] SKtokk { get; set; }
        [RfcEntityProperty("P_DATE")]
        // Format: yyyyMMdd
        public required string CutOffDate { get; set; }

    }
    
    internal class GetRFCPartialPaymentInputTable
    {
        [RfcEntityProperty("SIGN")]
        public required string SIGN { get; set; }
        [RfcEntityProperty("OPTION")]
        public required string OPTION { get; set; }
        [RfcEntityProperty("LOW")]
        public required string LOW { get; set; }
        [RfcEntityProperty("HIGH")]
        public required string HIGH { get; set; }
    }
}
