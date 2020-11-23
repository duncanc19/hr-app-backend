using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HRApp.API.Models
{
    public class VisitorDb
    {
        [Key]
        public Guid VisitorId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string EmployeeEmail { get; set; }
        public DateTime Appointment { get; set; }
    }
}