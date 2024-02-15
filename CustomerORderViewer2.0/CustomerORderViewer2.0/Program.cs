using CustomerORderViewer2._0.Models;
using CustomerORderViewer2._0.Repository;
using System;

namespace CustomerORderViewer2
{
    class Program
    {

        public delegate void FilterListDelegate<T>(params object[] parameters);

        private static string _connectionString  = @"Data Source=AZITD455;Initial Catalog=CustumerOrderViewer;Integrated Security=True;Encrypt=False;";
        private static readonly CustomerOrderCommand _customerOrderDetailCommand = new CustomerOrderCommand(_connectionString);
        private static readonly CustomerCommand _customerCommand = new CustomerCommand(_connectionString);
        private static readonly ItemCommand _itemCommand = new ItemCommand(_connectionString);
        private static IList<ItemModel> itemList = new List<ItemModel>(); // Capture the list within a closure

        static void Main(string[] args)
        {
            try
            {

                var contibueMapping = true;
                var userId = string.Empty;
                decimal minPrice = 0;
                string searchQuery = string.Empty;
                bool isValid = false;

                Console.WriteLine("What is ure username");
                userId = Console.ReadLine();

                do
                {
                    Console.WriteLine("1 - Show All ~  2 - Upsert Customer Order ~  3 - Delete Customer Order ~ 4 - CustomList ~ 5 - Exit  ");

                    int opion = Convert.ToInt32(Console.ReadLine());

                    switch (opion)
                    {
                        case 1:
                            ShowAll();
                            break;
                        case 2:
                            UpsertCustomerOrder(userId);
                            break;

                        case 3:
                            DeleteCustomer(userId); 
                            break;

                        case 4:
                            FilterListDelegate<ItemModel> filterItemsDelegate = FilterListByPrice;
                            filterItemsDelegate(minPrice);
                            break;
                       
                        case 5:
                            contibueMapping = false;
                            break;

                        case 6:
                            FilterListDelegate<CustomerModel> filterCustomersDelegate = FilterCustomers ;
                            filterCustomersDelegate(searchQuery);
                            break;

                        default:
                            Console.WriteLine("Option Not Availble");
                            contibueMapping = true;
                            break;
                    }
                }
                while (contibueMapping == true);
                
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Something Went wrong : {0}" , ex.Message);
            }
          
            
        }
        private static void ShowAll()
        {
            Console.WriteLine("{0}All Customer Order  : {1} ", Environment.NewLine, Environment.NewLine);
            DisplayCustomerOrders();

            Console.WriteLine("{0}All Customer   : {1} ", Environment.NewLine, Environment.NewLine);
            DisplayCustomers();

            Console.WriteLine("{0} All Items  : {1} ", Environment.NewLine, Environment.NewLine);
            DisplayItems();
        }

        private static void DisplayItems()
        {
            IList<ItemModel> items = _itemCommand.GetList();
            if (items.Any())
            {
                foreach(ItemModel item in items)
                {
                    Console.WriteLine("{0} : Description  {1}, Price {2} " , item.ItemId , item.Description, item.Price);
                }
            }
        }

        //private static void DisplayItemCustomList(decimal minPrice)
        //{    
        //    minPrice = Convert.ToDecimal(Console.ReadLine());

        //    IList<ItemModel> itemModels = _itemCommand.GetList().Where(x => x.Price > minPrice).ToList();

        //    foreach (ItemModel item in itemModels)
        //    {
        //        Console.WriteLine("{0} : Description {1}, Price {2}", item.ItemId, item.Description, item.Price);
        //    }
        //}

        private static void FilterListByPrice(params object[] parameters)
        {
            Console.WriteLine("Enter the minimum price of the items in the list wished:");
            decimal minPrice = (decimal)parameters[0];
            minPrice = Convert.ToDecimal(Console.ReadLine());  

            // Use the captured list for filtering
            IList<ItemModel> filteredItems = _itemCommand.GetList()
                            .Where(item => item.Price > minPrice).OrderBy(x => x.Price).ToList();

            foreach (ItemModel item in filteredItems)
            {
                Console.WriteLine("{0} : Description {1}, Price {2}", item.ItemId, item.Description, item.Price);
            }
        }

        private static void FilterCustomers(params object[] parameters)
        {
            Console.WriteLine("type to search for :");
             
            string searchQuery = Console.ReadLine().ToLower();

            // Use the captured list for filtering
            IList<CustomerModel> customerList = _customerCommand.GetList()
                            .Where(customer => customer.FirstName.ToLower().StartsWith(searchQuery)).ToList();

            foreach (CustomerModel customer in customerList)
            {
                Console.WriteLine("{0}  -  {1}", customer.LastName, customer.FirstName);
            }
        }



        private static void DisplayCustomers()
        {
            IList<CustomerModel> customers = _customerCommand.GetList();
            if (customers.Any())
            {
                foreach (CustomerModel customer in customers)
                {
                    Console.WriteLine("{0} : Name  {1}, midleName {2}  , lastname {3} , age  {4}",
                                        customer.CustomerId,
                                        customer.FirstName,
                                        customer.MidleName,
                                        customer.LastName,
                                        customer.age );
                }
            }
        }

        private static void DisplayCustomerOrders()
        {
            IList<CustomerOrderDetailModel> customerOrderDetails = _customerOrderDetailCommand.GetList();
            if (customerOrderDetails.Any())
            {
                foreach(CustomerOrderDetailModel customerOrderDetail in customerOrderDetails)
                {
                    Console.WriteLine(String.Format("{0}: Fullname: {1} {2} (Id: {3}) - purchased {4} for {5} (Id: {6})" ,
                                        customerOrderDetail.CustomerOrderId,
                                        customerOrderDetail.FirstName,
                                        customerOrderDetail.LastName,
                                        customerOrderDetail.CustomerOrderId,
                                        customerOrderDetail.Description,
                                        customerOrderDetail.Price,
                                        customerOrderDetail.ItemId));
                }
            }
        }

        private static void DeleteCustomer(string userId)
        {
            Console.WriteLine("enter customerOrderID :  ");
            int customerOrderId = Convert.ToInt32(Console.ReadLine());

            _customerOrderDetailCommand.Delete(customerOrderId , userId);
        }

        private static void UpsertCustomerOrder(string userId)
        {
            Console.WriteLine("Note: For updating insert existing CustomerOrderId, for new entries enter -1.");

            Console.WriteLine("Enter CustomerOrderId:");
            int newCustomerOrderId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CustomerId:");
            int newCustomerId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter ItemId:");
            int newItemId = Convert.ToInt32(Console.ReadLine());

           _customerOrderDetailCommand.Upsert(newCustomerOrderId, newCustomerId, newItemId, userId);
        }


    }
}