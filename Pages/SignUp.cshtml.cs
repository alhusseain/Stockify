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
    public string ID { get; set; }

    public SignUpModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Stockify;Data Source=LAPTOP-GTTG2OGR";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        if (IsIdAlreadyExists(connection, ID))
                        {
                            ModelState.AddModelError("ID", "Error with ID. This ID already exists.");
                            return Page();
                        }

                        string hashedPassword = HashPassword(Password);

                        string insertQuery = "INSERT INTO Signups (Username, Password, Employee_id) VALUES (@Username, @Password, @ID)";

                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Username", Username);
                            command.Parameters.AddWithValue("@Password", hashedPassword);
                            command.Parameters.AddWithValue("@ID", ID);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                return RedirectToPage("/SignIn");
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
            }

            return Page();
        }

    private bool IsIdAlreadyExists(SqlConnection connection, string id)
    {
        string checkQuery = "SELECT COUNT(*) FROM Signups WHERE Employee_id = @ID";
        
        using (SqlCommand command = new SqlCommand(checkQuery, connection))
        {
            command.Parameters.AddWithValue("@ID", id);

            int count = (int)command.ExecuteScalar();
            
            return count > 0;
        }
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
