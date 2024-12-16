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
        public required string SAPUserId { get; set; }
        public required string SAPKey { get; set; }
        [RfcEntityProperty("KTOKK")]
        public required GetRFCPartialPaymentInputTable AccountGroup {  get; set; }
        [RfcEntityProperty("P_DATE")]
        // Format: yyyyMMdd
        public required string CutOffDate { get; set; }

    }

    [RfcEntity("KTOKK")]
    internal class GetRFCPartialPaymentInputTable : ISapTable
    {
        [RfcEntityProperty("S_KTOKK")]
        // Z21 - Subcontractor
        // Z22 - Supplier Materials
        // Z23 - Supplier Plants Hiring
        // Z24 - Other Utilities
        public required string AccountType { get; set; }
    }
}
