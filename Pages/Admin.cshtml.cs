using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1.Pages
{
    [BindProperties(SupportsGet = true)]

    public class AdminModel : PageModel
    {

        private readonly ILogger<AdminModel> _logger;
        public DB db { get; set; }
        public DataTable EmployeeDataTable { get; set; }

        public Employee emplist { get; set; }

        public AdminModel(ILogger<AdminModel> logger, DB db)
        {
            _logger = logger;
            this.db = db;
        }

        public void OnGet()
        {
            EmployeeDataTable = db.ReadTable("Employee");
        }


        [BindProperty]
        public string EmployeeID { get; set; }

        [BindProperty]
        public string Fname { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Email { get; set; }


        [BindProperty]
        public string ManagerID { get; set; }

        [BindProperty]
        public string ManagerName { get; set; }

        [BindProperty]
        public string ManagerEmail { get; set; }

        [BindProperty]
        public string ManagerPhoneNumber { get; set; }



        public IActionResult OnPost()
            {
                string connection = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Stockify;Data Source=LAPTOP-GTTG2OGR";
                SqlConnection con = new SqlConnection(connection);
                string query = "";

                SqlCommand cmd = new SqlCommand(query, con);

                if(Request.Form["adminButton"].Count > 0)
                {
                    // Admin button was pressed
                    query = "insert into Employee(Fname, EmployeeID, RoleName, PhoneNumber, Email) values(@Fname, @EmployeeID, @RoleName, @PhoneNumber, @Email)";
                    cmd.Parameters.AddWithValue("@Fname", Fname);
                    cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    cmd.Parameters.AddWithValue("@RoleName", "Admin");
                    cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", Email);
                }
                else
                {
                    // Manager button was pressed
                    query = "insert into Employee(Fname, EmployeeID, RoleName, PhoneNumber, Email) values(@ManagerName, @ManagerID, @RoleName, @ManagerPhoneNumber, @ManagerEmail)";
                    cmd.Parameters.AddWithValue("@ManagerName", ManagerName);
                    cmd.Parameters.AddWithValue("@ManagerID", ManagerID);
                    cmd.Parameters.AddWithValue("@RoleName", "Manager");
                    cmd.Parameters.AddWithValue("@ManagerPhoneNumber", ManagerPhoneNumber);
                    cmd.Parameters.AddWithValue("@ManagerEmail", ManagerEmail);
                }

                cmd.CommandText = query;
                Console.Write(query);
                string res = "";

                try
                {
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                }
                catch (SqlException err)
                {
                    res = err.Message;
                }
                finally
                {
                    con.Close();
                }

                return RedirectToPage("/Admin");
            }
    }
}

