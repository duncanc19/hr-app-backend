using HRApp.API.Models;

namespace HRApp.API.IServices
{
    public interface IUserInfoService
    {   
        User Authenticate(string username, string password);
    }

}