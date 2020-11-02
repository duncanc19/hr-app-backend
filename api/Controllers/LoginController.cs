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
        List<Login> logins = new List<Login>() {
            new Login { Username = "Duncan", Password="abc" },
            new Login { Username = "Azlina", Password="def" },
            new Login { Username = "Joanna", Password="123"}

        };
        // POST api/login
        [HttpPost]
        public ActionResult<string> Post([FromBody] Login user)
        {
            var userfound = logins.SingleOrDefault( x => x.Username == user.Username && x.Password == user.Password);
            if (userfound != null)
            {
                return Ok(userfound);
            }
            return BadRequest(new {message="Username and password is incorrect"});
            
        }
    }
}