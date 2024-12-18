using SapCo2.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAISAPClient.SapRfcFunctions
{
    public class CreateCostPlanSQLV4(IRfcClient client)
    {
        public async Task<List<CreateCostPlanSQLV4OutputTable>?> ExecuteAsync(
            string projectId,
            string projectDescription,
            List<CreateCostPlanSQLV4InputTable> costplanitems)
        {
            //var costplanitems = new List<CreateCostPlanSQLV4InputTable>();
            //for (int i = 0; i < 1; i++)
            //{
            //    costplanitems.Add(new CreateCostPlanSQLV4InputTable
            //    {
            //        ProjectId = "1095",
            //        CompanyCode = "D007",
            //        CostCenter = "PROJ1095",
            //        ProjectStartDate = "20231115",
            //        ProjectEndDate = "20240528",
            //        CostCode = "0",
            //        CodeDescription = "Area 0 (m)",
            //        MFRItemNo = "",
            //    });
            //}

            var inputParameters = new CreateCostPlanSQLV4InputParameter()
            {
                ProjectId = projectId,
                Description = projectDescription,
                InTable = costplanitems.ToArray()
                //ProjectId = "1095",
                //Description = "Fitting-Out Works for 2/F Medical Center at 21 Ashley Road".Substring(0, 40),
                //InTable = costplanitems.ToArray()
            };

            var result = await client.ExecuteRfcAsync<CreateCostPlanSQLV4InputParameter, CreateCostPlanSQLV4OutputParameter>(AAISAPConstants.ZRFC135_CREATE_COSTPLAN_V4, inputParameters);
            Console.WriteLine($"ZRFC135_CREATE_COSTPLAN_V4 executed, retrieved record: {result?.outTable?.Length}");
            return result?.outTable?.ToList();
        }
    }
}
