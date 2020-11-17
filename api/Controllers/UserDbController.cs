using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;
using System.Reflection;
using Microsoft.AspNetCore.Cors;

namespace HRApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserDbController : ControllerBase
    {
        private readonly UserContext _userContext;
        public UserDbController(UserContext userContext)
        {
            _userContext = userContext;
            if (_userContext.User.Count() == 0)
            {
                _userContext.User.Add(new UserDb { 
                    Email = "admin@skillsforcare.org",
                    Password = "password",
                    Uuid = Guid.NewGuid(),
                    Id = 1
                });
                _userContext.SaveChanges();
            };
        }

        // GET api/userdb
        [HttpGet]
        public ActionResult<List<UserDb>> GetAll()
        {
            return _userContext.User.ToList();
        }

         // GET api/userdb/:id
        [HttpGet("{uuid}")]
        public ActionResult<UserDb> GetUserById(Guid uuid)
        {
            // var user = _userContext.User.Find(uuid);
            var user = _userContext.User.SingleOrDefault( x => x.Uuid == uuid );

            if (user == null) {
                return BadRequest(new {message = "ID does not exist"});
            }
            return Ok(user); 
        }

    //     // PUT api/user/:id
    //     [HttpPut("{id}")]
    //     public ActionResult<UserInfo> EditUserInfo(Guid id, [FromBody] UserInfo info)
    //     {
    //         var userfound = Users.SingleOrDefault( x => x.Id == id );
    //         if (userfound != null)
    //         {
    //             foreach (var item in typeof (UserInfo).GetProperties().Where(p => (p.GetValue(info) != null)))
    //             {
    //                 PropertyInfo property = typeof (UserInfo).GetProperty(item.Name);
    //                 if (!(property.PropertyType == typeof (DateTime) && property.GetValue(info).ToString() == new DateTime().ToString()))
    //                 {
    //                     property.SetValue(userfound.UserInfo, property.GetValue(info));
    //                 }
                  
    //             }
    //             return Ok(userfound.UserInfo); 
    //         }
    //         return BadRequest(new {message = "ID does not exist"});
    //     }
    

    //     // POST api/user/
    //     [HttpPost]
    //     public ActionResult<User> AddUser([FromBody] UserInfo info)
    //     {
    //         Guid id = Guid.NewGuid();
    //         string username = info.GenerateUsername();
    //         Login login = new Login { Username = username, Password = "ABC" };
    //         User user = new User (login, info, id);
    //         Users.Add(user);

    //         return Ok(Users); 
    //     }
    }
}