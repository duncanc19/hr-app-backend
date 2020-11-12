using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;
using Microsoft.AspNetCore.Cors;
using HRApp.API.IServices;
using Microsoft.IdentityModel.Tokens;

namespace HRApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LoginController : ControllerBase
    {
        private IUserInfoService _userInfoService;

        public LoginController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        // POST api/login
        [HttpPost]
        public ActionResult<string> Authenticate([FromBody] Login userLogin)
        {
            var user = _userInfoService.Authenticate(userLogin.Username, userLogin.Password);
            if (user == null) return BadRequest(new {message = "Username and password is incorrect"});
            // return Ok(new {Id= user.Id} );
            return Ok(user);
        }
    }
}