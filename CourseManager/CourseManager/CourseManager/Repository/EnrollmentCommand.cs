using CourseManager.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Repository
{
    class EnrollmentCommand
    {
        private string _connectionString;

        public EnrollmentCommand (string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<EnrollmentModel> GetList()
        {
            List<EnrollmentModel> enrollments = new List<EnrollmentModel>();

            var sql = "Enrollments_GetList";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                enrollments = connection.Query<EnrollmentModel>(sql).ToList();
            }


            return enrollments;
        }


        public void Upsert(EnrollmentModel enrollmentModel)
        {
            var sql = "Enrollments_Upsert";

            var userId = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("EnrollmentId" , typeof(int)));
            dataTable.Columns.Add(new DataColumn("StudentId" , typeof(int)));
            dataTable.Columns.Add(new DataColumn("CourseId" , typeof(int)));
            dataTable.Rows.Add(enrollmentModel.EnrollmentId , enrollmentModel.StudentId , enrollmentModel.CourseId);


            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { @EnrollmentType = dataTable.AsTableValuedParameter("EnrollmentType") , @UserId= userId},
                    commandType: CommandType.StoredProcedure);
            }

        }
    }
}
