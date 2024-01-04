using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    [BindProperties(SupportsGet = true)]
    public class EmployeesModel : PageModel
    {
        private readonly ILogger<EmployeesModel> _logger;
        private DB db;

        public DataTable EmployeeDataTable{ get; set; } =new DataTable("Employee");
        public Employee selected_emp { get; set; }

        public EmployeesModel(ILogger<EmployeesModel> logger)
        {
            _logger = logger;
            db = new DB();
        }

        public void OnGet()
        {
            EmployeeDataTable = db.ReadEmployeeData();
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
    }
}
