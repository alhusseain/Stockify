using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static WebApplication1.Pages.ProductsModel;

namespace WebApplication1.Pages
{
    public class ordersModel : PageModel
    {
        public List<orderinfo> orderinfoo { get; set; } = new List<orderinfo>();
        public int oid{get;set;}
        
        [BindProperty (SupportsGet = true)]
        public int current_i{get;set;}

        //int id{get;set;}

        // public ordersModel()
        // {
        //     id = OrderID;
        // }
        private int k;
        // public ordersModel()
        // {
        //     for(k=0;k<current_i;k++)
        //     {
        //         oid++;
        //     }
        // }


        public void OnGet()
        {
            try
            {
                List<int> orderid = new List<int>();
                string connectionString = "Server=DESKTOP-9IHIA03;Database=StockifyUpdated;Integrated Security=True;Encrypt=False;";
                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    connection1.Open();
                    string sqlQuery1 = "select OrderID from Orders";
                    using (SqlCommand command1 = new SqlCommand(sqlQuery1, connection1))
                    {
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                orderid.Add(Convert.ToInt32(reader1["OrderID"]));
                            }
                        }
                    }
                    //connection1.Close();
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "select o.barcode , p.Product_name , p.price , o.product_multiple from OrderInfo as o, Products as p where o.barcode = p.Barcode and o.order_id = " +orderid[current_i]+ " Order BY product_multiple Asc";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderinfo order = new orderinfo
                                {
                                    productID = reader["barcode"].ToString(),
                                    name = reader["Product_name"].ToString(),
                                    price = Convert.ToInt32(reader["price"]),
                                    quantity = Convert.ToInt32(reader["product_multiple"]),

                                };

                                orderinfoo.Add(order);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details (consider using a logging framework)
                Console.WriteLine("Exception: " + ex.ToString());
                throw; // Re-throw the exception to propagate it further if needed
            }
        }

    }

    public class orderinfo
    {
        public string productID { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }

    }
}
