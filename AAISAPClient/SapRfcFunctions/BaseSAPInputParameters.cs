using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions
{
    internal interface IBaseSAPInputParameters : IRfcInput
    {
        [RfcEntityProperty("SAP_USER_ID")]
        public string SAPUserId { get; set; }

        [RfcEntityProperty("SAP_Key")]
        public string SAPKey { get; set; }
    }
}
