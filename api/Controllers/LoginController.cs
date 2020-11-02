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
        // POST api/login
        [HttpPost]
        public ActionResult<string> Post([FromBody] Login user)
        {
            return "true";
        }
    }
}