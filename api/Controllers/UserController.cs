using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;
using System.Reflection;

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
        [HttpGet("{id}")]
        public ActionResult<UserInfo> Get(Guid id)
        {
            var userfound = Users.SingleOrDefault( x => x.Id == id );
            if (userfound != null)
            {
                return Ok(userfound.UserInfo); 
            }
            return BadRequest(new {message = "ID does not exist"});
            
        }

        // PUT api/user/:id
        [HttpPut("{id}")]
        public ActionResult<UserInfo> Put(Guid id, [FromBody] UserInfo info)
        {
            var userfound = Users.SingleOrDefault( x => x.Id == id );
            if (userfound != null)
            {
                foreach (var item in typeof (UserInfo).GetProperties().Where(p => (p.GetValue(info) != null)))
                {
                    PropertyInfo property = typeof (UserInfo).GetProperty(item.Name);
                    if (!(property.PropertyType == typeof (DateTime) && property.GetValue(info).ToString() == new DateTime().ToString()))
                    {
                        property.SetValue(userfound.UserInfo, property.GetValue(info));
                    }
                  
                }
                return Ok(userfound.UserInfo); 
            }
            return BadRequest(new {message = "ID does not exist"});
        }
    }
}