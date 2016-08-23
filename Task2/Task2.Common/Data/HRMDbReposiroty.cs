using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Task2.Common.Helpers;
using Task2.Common.Models;

namespace Task2.Common.Data
{
    public class HrmDbRepository
    {
        public static async Task<IList<SpReportModel>> GetSalaryReportNotAssigned(DateTime date, int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection())
            {
                var reader = await connection.ExecuteReaderAsync(String.Format("EXEC [dbo].[sp_SalaryReportNotAssigned] @Date = '{0:d}',@PageNumber = {1},@PageSize = {2}", date, pageNumber, pageSize), null, null, null, CommandType.StoredProcedure);

                return reader.MapToList<SpReportModel>();
            }
        }
    }
}
