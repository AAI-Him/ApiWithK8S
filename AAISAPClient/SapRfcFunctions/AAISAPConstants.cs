using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions
{
    internal class AAISAPConstants
    {
        public const string ZFI_RFC_PARTIAL_PAYMENT = "ZFI_RFC_PARTIAL_PAYMENT";
        public const string ZRFC135_CREATE_COSTPLAN_V4 = "ZRFC135_CREATE_COSTPLAN_V4";
    }

    public class SAPServiceConfiguration
    {
        public required string RfcName { get; set; }
        public required string QueueUrl { get; set; }
        public required string DLQueueUrl { get; set; }
        public int MaxRetryCount { get; set; }
    }

    public class AWSSQSChannelOption
    {
        public required string QueueUrl { get; set; }
        public required string DLQueueUrl { get; set; }
        public int MaxRetryCount { get; set; }
        public int MaxNumberOfMessages { get; set; }
    }
}
