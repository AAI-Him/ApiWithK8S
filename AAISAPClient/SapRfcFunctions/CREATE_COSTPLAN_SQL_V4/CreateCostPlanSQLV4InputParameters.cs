using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions.CREATE_COSTPLAN_SQL_V4
{
    public sealed class CreateCostPlanSQLV4InputParameters : IBaseSAPInputParameters
    {
        [RfcEntityProperty("Project_ID")]
        public required string ProjectId { get; set; }
        [RfcEntityProperty("Project_Desc")]
        public required string Description { get; set; }
        public required string SAPUserId { get; set; }
        public required string SAPKey { get; set; }
    }
}
