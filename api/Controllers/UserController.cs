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
           
                if (info.FirstName != null)
                {
                    userfound.UserInfo.FirstName = info.FirstName;
                }
                if (info.Surname != null)
                {
                    userfound.UserInfo.Surname = info.Surname;
                }
                if (info.Telephone != null)
                {
                    userfound.UserInfo.Telephone = info.Telephone;
                }
                if (info.Email != null)
                {
                    userfound.UserInfo.Email = info.Email;
                }
                
                
                // foreach (string key in info.Keys)
                // {
                //     PropertyInfo pinfo = typeof(UserInfo).GetProperty(key);
                //     object value = pinfo.GetValue(userfound, null);
                //     userfound.UserInfo.SetValue(key, info[key]);
                // }
                return Ok(userfound.UserInfo); 
            }
            return BadRequest(new {message = "ID does not exist"});

            // System.Reflection.PropertyInfo prop = typeof(UserInfo).GetProperty(key);

            // prop.SetValue(userfound.UserInfo, info[key]);
            
        }
    }
}