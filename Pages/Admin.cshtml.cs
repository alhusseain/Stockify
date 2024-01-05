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

    }
}

