using System;

namespace HRApp.API.Models
{
    public class UserInfo 
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Permission { get; set; }

    }
}
