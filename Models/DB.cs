using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.X86;

namespace WebApplication1.Models
{
    public class DB
{
    private string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=StockifyUpdated;Data Source=KAREEM";
    private SqlConnection con = new SqlConnection();

    public DB()
    {
        con.ConnectionString = connectionString;
    }

    public DataTable ReadTable(string table)
    {
        DataTable dt = new DataTable();
        string query = "select EmployeeId ,Fname, RoleName ,PhoneNumber, Branch_ID, Email from Employee";
        SqlCommand cmd = new SqlCommand(query, con);

        try
        {
            con.Open();
            dt.Load(cmd.ExecuteReader());

        }
        catch (SqlException err)
        {
            Console.WriteLine(err.Message);
        }
        finally
        {
            con.Close();
        }
        return dt;
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

        public bool AdminExists(string ID)
        {
            string query = $"SELECT COUNT(*) FROM Employee WHERE EmployeeID = '{ID}'";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
