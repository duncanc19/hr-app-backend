using System.Security.Claims;
using System.Text;
using HRApp.API.IServices;
using HRApp.API.Services;
using HRApp.API.Helpers;
using HRApp.API.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace HRApp.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly UserContext _userContext;

        public TokenService(IOptions<AppSettings> appSettings, UserContext userContext)
        {
            _appSettings = appSettings.Value;
            _userContext = userContext;
        }

        public Token Authenticate(string email, string password)
        {
            string savedPasswordHash = _userContext.User.SingleOrDefault(x => x.Email == email).Password;
            bool checkPassword = PasswordHash.ValidPassword(password, savedPasswordHash);
            
            if (!checkPassword)
            {
                return null;
            }
            
            var user = _userContext.User.SingleOrDefault(x => x.Email == email);
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            
            string env = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(env))
            {
                env = _appSettings.Secret;
            } 
            var key = Encoding.ASCII.GetBytes(env);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var newToken = new Token();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            newToken.Info = tokenHandler.WriteToken(token);
            return newToken;
        }
    }
}