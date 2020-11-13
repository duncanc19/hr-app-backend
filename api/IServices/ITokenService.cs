using HRApp.API.Models;

namespace HRApp.API.IServices
{
    public interface ITokenService
    {
        Token Authenticate(string username, string password);
    }

}