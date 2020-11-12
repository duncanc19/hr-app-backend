using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;
using System.Reflection;
using Microsoft.AspNetCore.Cors;
using HRApp.API.Services;

namespace HRApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private UserInfoService _userInfoService;

        public UserController(UserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        // GET api/user
        [HttpGet("{id}")]
        public ActionResult<UserInfo> Get(Guid id)
        {
            var userfound = _userInfoService._users.SingleOrDefault( x => x.Id == id );
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
            var userfound = _userInfoService._users.SingleOrDefault( x => x.Id == id );
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
    

        // POST api/user/
        [HttpPost]
        public ActionResult<User> Post([FromBody] UserInfo info)
        {
            Guid id = Guid.NewGuid();
            string username = info.GenerateUsername();
            Login login = new Login { Username = username, Password = "ABC" };
            User user = new User (login, info, id);
            _userInfoService._users.Add(user);

            return Ok(_userInfoService._users); 
        }
    }
}