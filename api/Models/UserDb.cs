using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HRApp.API.Models
{
    public class UserDb 
    {
        [Key]
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string PermissionLevel { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string NextOfKin { get; set; }
        public string Address { get; set; }
        public string Salary { get; set; }
        public string Password { get; set; }
        public string ManagerEmail { get; set; }
        public DateTime DoB { get; set; }
    }
}