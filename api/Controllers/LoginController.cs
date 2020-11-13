using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;
using Microsoft.AspNetCore.Cors;
using HRApp.API.IServices;

namespace HRApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LoginController : ControllerBase
    {
        List<User> Users { get; set; }
        ITokenService _tokenService { get; set; }

        public LoginController(ITokenService tokenService)
       {
           var userList = new UserList();
           Users = userList.users;
           _tokenService = tokenService;
       }

        // POST api/login
        [HttpPost]
        public ActionResult<string> Post([FromBody] Login user)
        {
            var userfound = Users.SingleOrDefault( x => x.Login.Username == user.Username && x.Login.Password == user.Password);
            if (userfound != null)
            {
                return Ok(new {Id= userfound.Id} ); 
            }
            return BadRequest(new {message = "Username and password is incorrect"});
             
        }

        // POST api/login/authenticate
        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] Login user)
        {
            var token = _tokenService.Authenticate(user.Username, user.Password);
            if (token == null) return BadRequest(new {message = "Username and password incorrect"});
            return Ok(new {token = token.Info});
            
        }

    }

}