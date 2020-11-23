// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using HRApp.API.IServices;
// using HRApp.API.Models;

// namespace HRApp.API.Helpers {

//   public class JwtMiddleware
//   {
//     private readonly RequestDelegate _next;
//     private readonly AppSettings _appSettings;

//     public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
//     {
//       _next = next;
//       _appSettings = appSettings.Value;
//     }

//     public async Task Invoke(HttpContext context, ITokenService tokenService)
//     {
//       var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//       Console.WriteLine("*******************");
//       Console.WriteLine(token);
//       if (token != null) attachUserToContext(context, tokenService, token);

//       await _next(context);
//     }

//     private void attachUserToContext(HttpContext context, ITokenService tokenService, string token)
//     {
//       try 
//       {
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
//         tokenHandler.ValidateToken(token, new TokenValidationParameters
//         {
//           ValidateIssuerSigningKey = true,
//           IssuerSigningKey = new SymmetricSecurityKey(key),
//           ValidateIssuer = false,
//           ValidateAudience = false,
//           ClockSkew = TimeSpan.Zero
//         }, out SecurityToken validatedToken);

//         var jwtToken = (JwtSecurityToken)validatedToken;
//         var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

//         // context.Items["User"] = tokenService.GetById(userId);
//       }
//       catch 
//       {

//       }
//     }
//   }
// }
