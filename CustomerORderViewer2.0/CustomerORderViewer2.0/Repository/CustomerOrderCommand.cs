using CustomerORderViewer2._0.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerORderViewer2._0.Repository
{
    class CustomerOrderCommand
    {
        private string _connectionString;

        public CustomerOrderCommand(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Upsert(int customerOrderId, int customerId , int itemdId, string userId)
        {
            var upsertStatement = "CustomerOrderDetail_Upsert";

            var dataTable = new DataTable();
            dataTable.Columns.Add("CustomerOrderId" , typeof(int));
            dataTable.Columns.Add("CustomerId", typeof(int));
            dataTable.Columns.Add("ItemId", typeof(int));
            dataTable.Rows.Add(customerOrderId , customerId , itemdId);

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(upsertStatement,
                        new { @CustomerOrderType = dataTable.AsTableValuedParameter("CustomerOrderType"), @UserId = userId  },
                        commandType: CommandType.StoredProcedure);
            }
        }
        public void Delete(int customerOrderId , string userId) 
        {
            var upsertStatement = "CustomerOrderDetail_Delete";
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(upsertStatement,
                    new {@CustomerOrderId = customerOrderId, @UserId = userId} ,
                    commandType : System.Data.CommandType.StoredProcedure);
            }
        }

        public IList<CustomerOrderDetailModel> GetList()
        {
            List<CustomerOrderDetailModel> customerOrderDetail = new List<CustomerOrderDetailModel>();

            var sql = "CustomerOrderDetail_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                customerOrderDetail = connection.Query<CustomerOrderDetailModel>(sql).ToList();
            }

            return customerOrderDetail;
        }
    }
}
