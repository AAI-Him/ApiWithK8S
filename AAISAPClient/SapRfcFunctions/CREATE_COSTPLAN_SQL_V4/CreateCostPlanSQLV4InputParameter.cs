using SapCo2.Abstraction;
using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions
{
    public sealed class CreateCostPlanSQLV4InputParameter : IBaseSAPInputParameters
    {
        [RfcEntityProperty("PROJECT_ID")]
        public required string ProjectId { get; set; }
        [RfcEntityProperty("PROJECT_DESC")]
        public required string Description { get; set; }
        [RfcEntityProperty("IN_TAB")]
        public CreateCostPlanSQLV4InputTable[]? InTable { get; set; }
    }
    
    public sealed class CreateCostPlanSQLV4InputTable
    {
        [RfcEntityProperty("PROJECT_ID")]
        public required string ProjectId { get; set; }
        [RfcEntityProperty("COMP_CDE")]
        public required string CompanyCode { get; set; }
        [RfcEntityProperty("COST_CENTER")]
        public required string CostCenter { get; set; }
        [RfcEntityProperty("PROJECT_SDTE")]
        public required string ProjectStartDate { get; set; }
        [RfcEntityProperty("PROJECT_EDTE")]
        public required string ProjectEndDate { get; set; }
        [RfcEntityProperty("COST_CDE")]
        public required string CostCode { get; set; }
        [RfcEntityProperty("CODE_DESC")]
        public required string CodeDescription { get; set; }
        [RfcEntityProperty("MFR_ITEM_NO")]
        public required string MFRItemNo { get; set; }
    }
}
