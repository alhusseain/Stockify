using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

public class ChangePasswordModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    [StringLength(8, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 8)]
    public string NewPassword { get; set; }

    [BindProperty]
    public string ConfirmNewPassword { get; set; }

    [BindProperty]
    public string Answer { get; set; }

    [BindProperty]
    public string Question { get; set; }

    public ChangePasswordModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnPost()
    {
        try
        {
            if (ModelState.IsValid)
            {
                string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=StockifyUpdated;Data Source=KAREEM";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (!IsUserValid(connection, Username, Answer, Question))
                    {
                        ModelState.AddModelError("Invalid", "Invalid username, answer, or question.");
                        return Page();
                    } 

                    if (NewPassword != ConfirmNewPassword)
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "The new password and confirmation do not match.");
                        return Page();
                    }

                    string hashedNewPassword = HashPassword(NewPassword);

                    if (UpdatePassword(connection, Username, hashedNewPassword))
                    {
                        return RedirectToPage("/SignIn"); 
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

    private bool IsUserValid(SqlConnection connection, string username, string answer, string question)
        {
            // Query for answer 
            string getAnswerQuery = "SELECT Answer FROM Signups WHERE Username = @Username";

            using (SqlCommand answerCommand = new SqlCommand(getAnswerQuery, connection))
            {
                answerCommand.Parameters.AddWithValue("@Username", username);
                object result1 = answerCommand.ExecuteScalar();

                if (result1 != null)
                {
                    string storedAnswer = result1.ToString();

                    if (storedAnswer == answer)
                    {
                        // Query for answer
                        string getQuestionQuery = "SELECT Question FROM Signups WHERE Username = @Username";

                        using (SqlCommand questionCommand = new SqlCommand(getQuestionQuery, connection))
                        {
                            questionCommand.Parameters.AddWithValue("@Username", username);
                            object result2 = questionCommand.ExecuteScalar();

                            if (result2 != null)
                            {
                                string storedQuestion = result2.ToString();

                                return storedQuestion == question;
                            }
                        }
                    }
                }

                return false; 
            }
        }





    private bool UpdatePassword(SqlConnection connection, string username, string hashedNewPassword)
    {
        string updateQuery = "UPDATE Signups SET Password = @Password WHERE Username = @Username";

        using (SqlCommand command = new SqlCommand(updateQuery, connection))
        {
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", hashedNewPassword);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
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

