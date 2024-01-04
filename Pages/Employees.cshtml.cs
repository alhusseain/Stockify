using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    [BindProperties(SupportsGet = true)]
    public class EmployeesModel : PageModel
    {
        private readonly ILogger<EmployeesModel> _logger;
        public DB db { get; set; }
        public DataTable EmployeeDataTable { get; set; }
        public Employee selected_emp { get; set; }
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
    }
}
