using CustomerORderViewer2._0.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerORderViewer2._0.Repository
{
    class CustomerCommand
    {
        private string _connectionString;

        public CustomerCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<CustomerModel> GetList()
        {
            List<CustomerModel> custommers = new List<CustomerModel>();

            var sql = "Customer_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                custommers = connection.Query<CustomerModel>(sql).ToList();


            }

            return custommers;
        }
    }
}
