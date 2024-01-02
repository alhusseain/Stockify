using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;

public class SignUpModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string Role { get; set; }

    [BindProperty]
    public string ID { get; set; }

    public SignUpModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            string connectionString = "Server=DESKTOP-9IHIA03;Database=StockifyUpdated;Integrated Security=True;Encrypt=True;";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string hashedPassword = HashPassword(Password);

                string insertQuery = "INSERT INTO Signups (Username, Password, Role, Employee_id) VALUES (@Username, @Password, @Role, @ID)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@ID", ID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return RedirectToPage("/SignIn");
                    }
                }
            }
        }

        return Page();
    }

    private string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return $"{Convert.ToBase64String(salt)}:{hashed}";
    }
}
