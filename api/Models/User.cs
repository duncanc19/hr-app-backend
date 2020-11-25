using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HRApp.API.Models
{
    public class User 
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string AdminLevel { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string NextOfKin { get; set; }
        public string Address { get; set; }
        public string Salary { get; set; }
        public string ManagerEmail { get; set; }
        public DateTime DoB { get; set; }
        public string Password { get; set; }
    }
}