using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace WebApplication1.Pages
{
    // WebApplication1.Pages.productsModel.cshtml.cs

  

    public class ProductsModel : PageModel
    {
        [BindProperty]
        public required string Role_products { get; set; }
        // Example product data for each category
        private Dictionary<string, List<Product>> products = new Dictionary<string, List<Product>>()
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

        public void OnGet()
        {
            // Default category to display initially

            string defaultCategory = "grocery";
            ViewData["DefaultCategory"] = defaultCategory;
            DisplayProducts(defaultCategory);
        }

        public IActionResult OnPostButtonClick(string productName, bool isIncrement)
        {
            // Find the product and update the quantity
            string category = ViewData["DefaultCategory"] as string;
            var product = products[category].Find(p => p.Name == productName);

            if (product != null)
            {
                if (isIncrement && product.Quantity > 0)
                {
                    product.Quantity--;
                }
                else if (!isIncrement && product.Quantity < product.TotalQuantity)
                {
                    product.Quantity++;
                }

                return new JsonResult(new { success = true, quantity = product.Quantity });
            }

            return new JsonResult(new { success = false });
        }

        private void DisplayProducts(string category)
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
