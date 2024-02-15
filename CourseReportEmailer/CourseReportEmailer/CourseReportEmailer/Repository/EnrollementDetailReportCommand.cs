using CourseReportEmailer.Module;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CourseReportEmailer.Repository
{
    class EnrollmentDetailReportCommand
    {
        private string _connectionString;

        public EnrollmentDetailReportCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<EnrollementDetailReportModel> GetList()
        {
            List<EnrollementDetailReportModel> enrollmentDetailReport = new List<EnrollementDetailReportModel>();
            var sql = "EnrollmentReport_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                enrollmentDetailReport = connection.Query<EnrollementDetailReportModel>(sql).ToList();
            }

            return enrollmentDetailReport;
        }

    }
}