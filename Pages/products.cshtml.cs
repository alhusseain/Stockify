using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WebApplication1.Pages
{
    // WebApplication1.Pages.productsModel.cshtml.cs

  

    public class ProductsModel : PageModel
    {
        [BindProperty]
        public required string Role_products { get; set; }
        // Example product data for each category
        public Dictionary<string, List<Product>> products { get; set; }


        public List<string> categories { get; set; }= new List<string>();

        [BindProperty]
        public int Barcode{ get; set;}
        [BindProperty]
        public float Price{ get; set;}
        [BindProperty]
        public int Quantity{ get; set;}
        [BindProperty]
        public string Name{ get; set;}
        [BindProperty]
        public string Image{ get; set;}
        [BindProperty]
        public int Max{ get; set;}
        [BindProperty]
        public int Recorder_level{ get; set;}

        [BindProperty]
        public string supply_code{ get; set;}
        public string connectionstring="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=StockifyUpdated;Data Source=DESKTOP-9IHIA03";
        
        public void OnGet()
        {
            // Default category to display initially
            products = new Dictionary<string, List<Product>>()
            {
                {
                    "grocery", new List<Product>
                    {
                        new Product { Name = "Rice", Image = "/static/doha.jpg", Quantity = 5, Company = "Al Doha Foods" },
                        new Product { Name = "Crystal Oil", Image = "/static/oil.png", Quantity = 5, Company = "Arma For food industries" },
                        // Add more products as needed
                    }
                },
                {
                    "snacks", new List<Product>
                    {
                        new Product { Name = "Corona Hazelnut Chocolate", Image = "/static/corona1.png", Quantity = 13, Company = "Corona Co." },
                        new Product { Name = "Corona Milk Chocolate", Image = "/static/corona2.png", Quantity = 13, Company = "Corona Co." },
                        new Product { Name = "Corona Dark Chocolate", Image = "/static/corona3.png", Quantity = 13, Company = "Corona Co." },
                    }
                },
                {
                    "drinks", new List<Product>
                    {
                        new Product { Name = "Spiro Spathis Apple", Image = "/static/spiro1.png", Quantity = 20, Company = "Spiro Spathis Co." },
                        new Product { Name = "Spiro Spathis Lemonade", Image = "/static/spiro2.png", Quantity = 15, Company = "Spiro Spathis Co." },
                        new Product { Name = "Spiro Spathis Grapes", Image = "/static/spiro3.png", Quantity = 12, Company = "Spiro Spathis Co." },
                        new Product { Name = "Spiro Spathis Mandarin", Image = "/static/spiro4.png", Quantity = 18, Company = "Spiro Spathis Co." },
                        new Product { Name = "Spiro Spathis Peach", Image = "/static/spiro5.png", Quantity = 25, Company = "Spiro Spathis Co." },
                        new Product { Name = "Spiro Spathis Pineapple", Image = "/static/spiro6.png", Quantity = 30, Company = "Spiro Spathis Co." },
                        new Product { Name = "Spiro Spathis Kiwi", Image = "/static/spiro7.png", Quantity = 10, Company = "Spiro Spathis Co." },
                        // Add more drinks as needed
                    }
                }
        // Add more categories with their respective products
            };
            categories.Add("grocery");
            categories.Add("snacks");
            categories.Add("drinks");

        }

        public IActionResult OnPost()
        {
            string query="insert into Products(Barcode,supplier_code,price,Product_name,Decription,Max_number,current_number,Recorder_level) values(@Barcode,@supplier_code,@price,@Product_name,@Description,@Max_number,@Recorder_level,@Recorder_level)";
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Barcode", Barcode);
            command.Parameters.AddWithValue("@supplier_code", supply_code);
            command.Parameters.AddWithValue("@price", Price);
            command.Parameters.AddWithValue("@Product_name", Name);
            command.Parameters.AddWithValue("@Description", Image);
            command.Parameters.AddWithValue("@Max_number", Max);
            command.Parameters.AddWithValue("@Recorder_level", Recorder_level);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToPage("/products");
            
        }


        private void DisplayProducts(string category, Dictionary<string, List<Product>> products)
        {
            ViewData["Category"] = category;
            ViewData["Products"] = products[category];
        }

         public class Product
         {
             public string Name { get; set; }
             public string Image { get; set; }
             public int Quantity { get; set; }
             public string Company { get; set; }

             // Add any other necessary properties, e.g., TotalQuantity
             public int TotalQuantity { get; set; }
        }
    }

}
