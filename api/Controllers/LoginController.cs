using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;

namespace HRApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        List<User> Users { get; set; }

       public LoginController()
       {
           var userList = new UserList();
           Users = userList.users;
       }

        // POST api/login
        [HttpPost]
        public ActionResult<string> Post([FromBody] Login user)
        {
            var userfound = Users.SingleOrDefault( x => x.Login.Username == user.Username && x.Login.Password == user.Password);
            if (userfound != null)
            {
                return Ok(userfound.UserInfo); 
            }
            return BadRequest(new {message = "Username and password is incorrect"});
            
        }
    }
}