using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRApp.API.Models
{
    public class UserInfo 
    {

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
        public DateTime DoB { get; set; }

        public string GenerateUsername()
        {
            return this.FirstName + this.Surname[0];
        }
    }
}