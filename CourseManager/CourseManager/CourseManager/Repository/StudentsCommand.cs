using CourseManager.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseManager.Repository
{
    class StudentsCommand
    {
        private string _connectionString;

        public StudentsCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<StudentModel> GetList()
        {
            List<StudentModel> students = new List<StudentModel>();

            var sql = "Student_GetList";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // students = connection.Query<StudentModel>(sql).ToList();
                 students = connection.Query<StudentModel>(sql).ToList();

            }

            return students;
        }

    }
}
