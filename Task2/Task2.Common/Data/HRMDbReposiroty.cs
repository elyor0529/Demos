using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Task2.Common.Models;

namespace Task2.Common.Data
{
    public static class HrmDbRepository
    {
        public static async Task<IEnumerable<SpReportModel>> GetSalaryReportNotAssigned(DateTime date, int pageNumber,
            int pageSize)
        {
            using (
                var connection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<SpReportModel>("[dbo].[sp_SalaryReportNotAssigned]",
                            new
                            {
                                Date = string.Format("'{0}/{1}/{2}'", date.Month, date.Day, date.Year),
                                PageNumber = pageNumber,
                                PageSize = pageSize
                            }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}