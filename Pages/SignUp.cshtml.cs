using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

public class SignUpModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    [StringLength(8, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 8)]
    public string Password { get; set; }

    [BindProperty]
    public string ConfirmPassword { get; set; }

    [BindProperty]
    public string ID { get; set; }

    [BindProperty]
    public string Answer { get; set; }

    [BindProperty]
    public string Question { get; set; }

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
                if (Password != ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return Page();
                }

                string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=StockifyUpdated;Data Source=KAREEM";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (IsIdAlreadyExists(connection, ID))
                    {
                        ModelState.AddModelError("ID", "Error with ID. This ID already exists.");
                        return Page();
                    }

                    string hashedPassword = HashPassword(Password);

                    string insertQuery = "INSERT INTO Signups (Username, Password, Employee_id, Answer, Question) VALUES (@Username, @Password, @ID, @Answer, @Question)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@ID", ID);
                        command.Parameters.AddWithValue("@Answer", Answer);
                        command.Parameters.AddWithValue("@Question", Question);

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
