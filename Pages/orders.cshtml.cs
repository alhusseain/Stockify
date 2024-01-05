using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static WebApplication1.Pages.ProductsModel;

namespace WebApplication1.Pages
{
    public class ordersModel : PageModel
    {
        public List<orderinfo> orderinfoo { get; set; } = new List<orderinfo>();

        public async Task OnGetAsync()
        {
            try
            {
                string connectionString = "Server=DESKTOP-9IHIA03;Database=StockifyUpdated;Integrated Security=True;Encrypt=False;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sqlQuery = "select o.barcode , p.Product_name , p.price , o.product_multiple from OrderInfo as o, Products as p where o.barcode = p.Barcode Order BY product_multiple Asc";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
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
