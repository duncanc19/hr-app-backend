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
        ITokenService _tokenService { get; set; }
        private readonly UserContext _userContext;

        public LoginController(ITokenService tokenService, UserContext userContext)
        {
            _userContext = userContext;
            _tokenService = tokenService;
        }

        // POST api/login
        [HttpPost]
        public ActionResult<string> Post([FromBody] Login user)
        {
            // var userfound = _userContext.User.Find( x => x.Email == user.Email && x.Password == user.Password );
            var userfound = _userContext.User.SingleOrDefault( x => x.Email == user.Email && x.Password == user.Password);
            if (userfound != null)
            {
                return Ok(new {Id = userfound.UserId} ); 
            }
            return BadRequest(new {message = "Email or password is incorrect"});
        }

        // POST api/login/authenticate
        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] Login user)
        {
            var token = _tokenService.Authenticate(user.Email, user.Password);
            if (token == null) return BadRequest(new {message = "Email or password incorrect"});
            
            return Ok(new {token = token.Info}); 
        }

    }

}