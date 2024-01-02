using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;

namespace WebApplication1.Pages
{
    public class EmployeesModel : PageModel
    {
        public List<EmployeeInfo> Employee = new List<EmployeeInfo>();


        public void OnGet( )
        {
            try
            {
                string connecectionstring = "Server=DESKTOP-9IHIA03;Database=StockifyUpdated;Integrated Security=True;Encrypt=True;";

                using (SqlConnection connection = new SqlConnection(connecectionstring))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM Employee";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employee = new EmployeeInfo();

                                employee.Fname =  reader.GetString(0);
                                employee.EmployeeID = reader.GetInt32(1);
                                employee.RoleName = reader.GetString(2);
                                employee.Phonenumber = reader.GetString(8);
                                employee.Branch_ID = reader.GetInt32(9);

                                Employee.Add(employee);


                            }
                        }

                    }
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception" + ex.ToString());
            }
            finally
            {

            }
        }

       
    }

    public class EmployeeInfo
    {
        public string Fname;
        public int EmployeeID;
        public string RoleName;
        public string Phonenumber;
        public int Branch_ID;
        
    }
}

