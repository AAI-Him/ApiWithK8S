using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions
{
    internal class CreateCostPlanSQLV4OutputParameter : BaseSAPOutputParameters
    {
        [RfcEntityProperty("OUT_TAB")]
        public CreateCostPlanSQLV4OutputTable[]? outTable { get; set; }
    }

    public sealed class CreateCostPlanSQLV4OutputTable
    {
        [RfcEntityProperty("CLIENT")]
        public string? Client { get; set; }
        [RfcEntityProperty("MESSAGE_TYPE")]
        public string? MessageType { get; set; }
        [RfcEntityProperty("MESSAGE")]
        public string? Message { get; set; }
    }
}
