using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    [BindProperties(SupportsGet = true)]
    public class Employee
    {
        [Required]
        public string? EmployeeID { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string? Fname { get; set; }

        public string? RoleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Branch_ID { get; set; }
    }
}

