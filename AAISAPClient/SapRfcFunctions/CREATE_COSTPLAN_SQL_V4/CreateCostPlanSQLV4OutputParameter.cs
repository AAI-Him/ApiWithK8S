using SapCo2.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions.CREATE_COSTPLAN_SQL_V4
{
    internal class CreateCostPlanSQLV4OutputParameter : BaseSAPOutputParameters
    {
        [RfcEntityProperty("Project_ID")]
        public required string ProjectId { get; set; }
        [RfcEntityProperty("Comp_Cde")]
        public required string CompanyCode { get; set; }
        [RfcEntityProperty("Cost_Center")]
        public required string CostCenter { get; set; }
        [RfcEntityProperty("Project_Sdte")]
        public required string ProjectStartDate { get; set; }
        [RfcEntityProperty("Project_Edte")]
        public required string ProjectEndDate { get; set; }
        [RfcEntityProperty("Cost_Cde")]
        public required string CostCode { get; set; }
        [RfcEntityProperty("Code_desc")]
        public required string CostCodeDescription { get; set; }
        [RfcEntityProperty("MFR_Item_no")]
        public required string MFRItemNo { get; set; }
    }
}
