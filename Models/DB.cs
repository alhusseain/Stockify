using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.X86;

namespace WebApplication1.Models
{
    public class DB
    {
        private string connectionString = "Server=Victusx15\\DATABASE_SERVER;Database=StockifyUpdated;Integrated Security=True;Encrypt=False;";
        private SqlConnection con = new SqlConnection();

        public DB()
        {
            con.ConnectionString = connectionString;

        }

        public DataTable ReadEmployeeData()
        {
            DataTable dt = new DataTable();

            try
            {
                string connecectionstring = "Server=Victusx15\\DATABASE_SERVER;Database=StockifyUpdated;Integrated Security=True;Encrypt=False;";

                using (SqlConnection connection = new SqlConnection(connecectionstring))
                {
                    connection.Open();
                    string sqlQuery = "SELECT ";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
            }

            return dt;
        }


        public string AddEmployee(Employee emp)
        {
            string query = $"insert into Employee(Fname, EmployeeID, RoleName, PhoneNumber, Branch_ID) " +
                $"values('{emp.Fname}', '{emp.EmployeeID}', '{emp.RoleName}', '{emp.PhoneNumber}','{emp.Branch_ID}')";

            SqlCommand cmd = new SqlCommand(query, con);
            string res = "";    // this is for storing error messages (if any) and returning them from the function 

            try
            {
                con.Open();
                res = cmd.ExecuteNonQuery().ToString();
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
                res = err.Message;
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public string DeleteEmployee(string EmployeeID)
        {
            string msg = "";
            string query = $"delete from Employee where EmployeeID={EmployeeID}";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                msg = cmd.ExecuteNonQuery().ToString();

            }
            catch (SqlException err)
            {
                msg = err.Message;
            }
            finally
            {
                con.Close();
            }
            return msg;
        }

        public string UpdateEmployee(Employee emp)
        {

            string msg = "";
            string query = $"update Employee set Fname='{emp.Fname}', RoleName='{emp.RoleName}', PhoneNumber='{emp.PhoneNumber}, Branch_ID='{emp.Branch_ID} ' where EmployeeID={emp.EmployeeID}";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                con.Open();
                msg = cmd.ExecuteNonQuery().ToString();

            }
            catch (SqlException err)
            {
                msg = err.Message;
            }
            finally
            {
                con.Close();
            }
            return msg;
        }

        public Employee? getEmployee(string EmployeeID)
        {
            string msg = "";
            string query = $"select * from Employee where EmployeeID={EmployeeID}";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();

            Employee employee = new Employee();

            try
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());

                // handle not found employee
                if (dt.Rows.Count == 0)
                {
                    msg = "No employee with this ssn found";
                    Console.WriteLine(msg);
                    return null;
                }
               

                // transfer data from returned datatable into employee object
                employee.Fname = dt.Rows[0]["Fname"].ToString();
                employee.EmployeeID = dt.Rows[0]["EmployeeID"].ToString();
                employee.RoleName = dt.Rows[0]["RoleName"].ToString();
                employee.PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString();  // minit is coming from db as string, but it's a char in the Employee.cs, so I'm just converting from string to char here.
                employee.Branch_ID = dt.Rows[0]["Branch_ID"].ToString();
            }
            catch (SqlException err)
            {
                msg = err.Message;
                Console.WriteLine(msg);
            }
            finally
            {
                con.Close();
            }
            return employee;

        }
    }
}
