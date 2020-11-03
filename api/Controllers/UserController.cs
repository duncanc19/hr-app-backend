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
    public class UserController : ControllerBase
    {
        List<User> Users { get; set; }

       public UserController()
       {
           var userList = new UserList();
           Users = userList.users;
       }

        // GET api/user
        [HttpGet]
        public ActionResult<UserInfo> Get(Guid id)
        {
            var userfound = Users.SingleOrDefault( x => x.Id == id );
            if (userfound != null)
            {
                return Ok(userfound.UserInfo); 
            }
            return BadRequest(new {message = "ID does not exist"});
            
        }
    }
}