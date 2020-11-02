using System;

namespace HRApp.API.Models
{
    public class User
    {
        
        public Login Login { get; }
        public UserInfo UserInfo { get; } 

        public Guid Id { get; set; }

        public User(Login login, UserInfo info, Guid id)
        {
            this.Login = login;
            this.UserInfo = info;
            this.Id = id;
        }
    }
}