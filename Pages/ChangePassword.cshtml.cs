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
                string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Stockify;Data Source=LAPTOP-GTTG2OGR";

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
                        return RedirectToPage("/SignIn"); // Redirect to sign-in page after successful password change
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
            // Query for retrieving the answer from the database
            string getAnswerQuery = "SELECT Answer FROM Signups WHERE Username = @Username";

            using (SqlCommand answerCommand = new SqlCommand(getAnswerQuery, connection))
            {
                answerCommand.Parameters.AddWithValue("@Username", username);
                object result1 = answerCommand.ExecuteScalar();

                if (result1 != null)
                {
                    // Retrieve the answer from the database
                    string storedAnswer = result1.ToString();

                    // Check if the provided answer matches the stored answer
                    if (storedAnswer == answer)
                    {
                        // Query for retrieving the question from the database
                        string getQuestionQuery = "SELECT Question FROM Signups WHERE Username = @Username";

                        using (SqlCommand questionCommand = new SqlCommand(getQuestionQuery, connection))
                        {
                            questionCommand.Parameters.AddWithValue("@Username", username);
                            object result2 = questionCommand.ExecuteScalar();

                            if (result2 != null)
                            {
                                // Retrieve the question from the database
                                string storedQuestion = result2.ToString();

                                // Check if the provided question matches the stored question
                                return storedQuestion == question;
                            }
                        }
                    }
                }

                return false; // Username not found in the database or incorrect answer
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

