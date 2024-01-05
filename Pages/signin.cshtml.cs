using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Data.SqlClient;



namespace WebApplication1.Pages
{
    public class signinModel : PageModel
    {
        private readonly ILogger<signinModel> _logger;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public signinModel(ILogger<signinModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=StockifyUpdated;Data Source=DESKTOP-9IHIA03";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Password FROM Signups WHERE Username = @Username";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedPassword = result.ToString();
                            string[] parts = storedPassword.Split(':');
                            string storedSalt = parts[0];
                            string storedHashedPassword = parts[1];

                            string hashedEnteredPassword = HashPassword(Password, Convert.FromBase64String(storedSalt));

                            if (hashedEnteredPassword == storedHashedPassword)
                            {
                                return RedirectToPage("/home");
                            }
                        }
                    }
                }
            }

            return Page();
        }

        private string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
