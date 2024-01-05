using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using WebApplication1.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace WebApplication1.Pages
{
    [BindProperties(SupportsGet = true)]

    public class EmployeesModel : PageModel
    {



        private readonly ILogger<EmployeesModel> _logger;
        public DB db { get; set; }
        public DataTable EmployeeDataTable { get; set; }

        public Employee emplist { get; set; }

        public EmployeesModel(ILogger<EmployeesModel> logger, DB db)
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
        public string Branch_ID { get; set; }



        public IActionResult OnPostInsert()
        {

            string connection = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Stockify;Data Source=LAPTOP-GTTG2OGR";
            SqlConnection con = new SqlConnection(connection);
            string query = "insert into Employee(Fname, EmployeeID, RoleName, PhoneNumber, Branch_ID) values(@Fname, @EmployeeID,@RoleName,@PhoneNumber,@Branch_ID)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Fname", Fname);
            cmd.Parameters.AddWithValue("@EmployeeID", int.Parse(EmployeeID));
            cmd.Parameters.AddWithValue("@RoleName", RoleName);
            cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            cmd.Parameters.AddWithValue("@Branch_ID", int.Parse(Branch_ID));
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
            return RedirectToPage("/Employees");

        }
    }
}

