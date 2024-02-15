using CustomerORderViewer2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;


namespace CustomerORderViewer2._0.Repository
{
    class ItemCommand
    {
        private string _connectionString;

        

        public ItemCommand(string connectionString)
        {
            _connectionString = connectionString;
        }   

        public IList<ItemModel> GetList()
        {
            List<ItemModel> items = new List<ItemModel>();

            var sql = "Item_GetList";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                items = connection.Query<ItemModel>(sql).ToList();
            }

                return items;
        }
    }
}
