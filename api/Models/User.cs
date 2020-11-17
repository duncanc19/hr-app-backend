using System;
using System.ComponentModel.DataAnnotations;

namespace HRApp.API.Models
{
    public class User
    {
        private const string V = "";
        
        [Key]
        public Guid Id { get; set; }
        public Login Login { get; set; }
        public UserInfo UserInfo { get; set; } 
        public Token Token {get; set;} 

        public User(Login login, UserInfo info, Guid id)
        {
            this.Login = login;
            this.UserInfo = info;
            this.Id = id;
            this.Token = new Token();
        }
    }
}