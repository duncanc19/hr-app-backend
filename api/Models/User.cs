namespace HRApp.API.Models
{
    public class User
    {
        
        public Login Login { get; }
        public UserInfo UserInfo { get; } 

        public User(Login login, UserInfo info)
        {
            this.Login = login;
            this.UserInfo = info;
        }
    }
}