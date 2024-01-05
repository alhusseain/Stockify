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
        public string RoleName { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public string Email { get; set; }



        public IActionResult OnPost()
        {

            string connection = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Stockify;Data Source=LAPTOP-GTTG2OGR";
            SqlConnection con = new SqlConnection(connection);
            string query = "insert into Employee(Fname, EmployeeID, RoleName, PhoneNumber, Email) values(@Fname, @EmployeeID,@RoleName,@PhoneNumber,@Email)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Fname", Fname);
            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmd.Parameters.AddWithValue("@RoleName", "Admin");
            cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", Email);
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

