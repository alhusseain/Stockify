using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.SignalR;
using System.Reflection.Metadata;

namespace WebApplication1.Pages
{

    public class EmployeesModel : PageModel
    {
        private readonly ILogger<EmployeesModel> _logger;
        public DB db { get; set; } = new DB();
        public DataTable EmployeeDataTable { get; set; }
        public Employee employee { get; set; }
        [BindProperty]
        public string employeeId { get; set; }
        [BindProperty]
        public string name { get; set; }
        [BindProperty]
        public string worksAs { get; set; }
        [BindProperty]
        public string phoneNumber { get; set; }
        [BindProperty]
        public string branch { get; set; }

        public EmployeesModel(ILogger<EmployeesModel> logger, DB db)
        {
            _logger = logger;
            this.db = db;
        }

        public void OnGet()
        {
            EmployeeDataTable = db.ReadTable("Employee");
        }

        public IActionResult OnPostDelete(string employeeId)
        {
            // Your delete logic
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostUpdate(Employee emp)
        {
            // Your update logic
            return new JsonResult(new { success = true });
        }


        public void OnPost()
        {
            // employee.EmployeeID=employeeId;
            // employee.Fname=name;
            // employee.RoleName=worksAs;
            // employee.PhoneNumber=phoneNumber;
            // employee.Branch_ID=branch;
            // db.AddEmployee(employee);
            // EmployeeDataTable = db.ReadTable("Employee");
            string connection = "Server=DESKTOP-9IHIA03;Database=StockifyUpdated;Integrated Security=True;Encrypt=False;";
            SqlConnection con = new SqlConnection(connection);
            string query = "insert into Employee(Fname, EmployeeID, RoleName, PhoneNumber, Branch_ID) values(@Fname, @EmployeeID,@RoleName,@PhoneNumber,@Branch_ID)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Fname", name);
            cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(employeeId));
            cmd.Parameters.AddWithValue("@RoleName", worksAs);
            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@Branch_ID", int.Parse(branch));
            Console.Write(query);
            string res = "";    // this is for storing error messages (if any) and returning them from the function 

            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery().ToString();
                //return RedirectToPage("Employees");
                //db.AddEmployee(employee);
                //EmployeeDataTable = db.ReadTable("Employee");
            }
            catch (SqlException err)
            {
                res = err.Message;
            }
            finally
            {
                con.Close();
            }
            //return Page();

        }
    }
}