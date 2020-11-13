using System.Security.Claims;
using System.Text;
using HRApp.API.IServices;
using HRApp.API.Helpers;
using HRApp.API.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.Extensions.Options;

namespace HRApp.API.Services
{
    public class TokenService : ITokenService
    {
        
        // private List<User> _users = new List<User> {
        //     // new User { 
        //     //     Id = new Guid("c0a68046-617e-4927-bd9b-c14ce8f497e1"),
        //     //     Login = new Login { Username = "Duncan", Password="abc" },
        //     //     UserInfo = new UserInfo { FirstName = "Duncan", Surname = "Carter", Role = "Employee", PermissionLevel = "Default",
        //     //             Telephone = "0771333333333", Email = "duncan@dc.com", Location = "Paris", NextOfKin = "Mother", Address = "Champs Elysee",
        //     //             Salary = "£56000", DoB = new DateTime(1912,01,01) },
        //     //     Token = new Token {Info= ""}
        //     // },
        //     // new User {
        //     //     Id = new Guid("18712a4f-744e-4e7c-a191-395fa832518b"),
        //     //     Login= new Login { Username = "Azlina", Password="def" },
        //     //     UserInfo= new UserInfo { FirstName = "Azlina", Surname = "Yeo", Role = "Employee", PermissionLevel = "Default",
        //     //             Telephone = "0771333546433", Email = "azlina@happy.com", Location = "Singapore", NextOfKin = "Father", Address = "Bedok Reservoir Road",
        //     //             Salary = "£29000", DoB = new DateTime(1979,01,01) },
        //     //     Token = new Token {Info=""}
        //     // },
        //     // new User {
        //     //     Id = new Guid("6d56e5bd-bba7-4026-9e3d-383f2c2f8d4d"),
        //     //     Login = new Login { Username = "Joanna", Password="123" },
        //     //     UserInfo = new UserInfo { FirstName = "Joanna", Surname = "Fawl", Role = "Employee", PermissionLevel = "Default",
        //     //             Telephone = "07713344333", Email = "joanna@jf.com", Location = "Seattle", NextOfKin = "Brother", Address = "The Tower",
        //     //             Salary = "£75000", DoB = new DateTime(1917,05,08) },
        //     //     Token = new Token {Info=""}
        //     // }

        // };
   
        private readonly AppSettings _appSettings;
        private List<User> _users { get; set; }

        public TokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            var userList = new UserList();
            _users = userList.users;
        }

        public Token Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x =>
            {
                return x.Login.Username == username && x.Login.Password == password;
            });
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token.Info = tokenHandler.WriteToken(token);

            return user.Token;
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

    }
}