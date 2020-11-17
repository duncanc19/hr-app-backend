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
                    AdminLevel = "admin",
                    UserId = Guid.NewGuid()
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
        [HttpGet("{userId}")]
        public ActionResult<UserDb> GetUserById(Guid userId)
        {
            var user = _userContext.User.Find(userId);

            if (user == null) {
                return BadRequest(new {message = "ID does not exist"});
            }
            return Ok(user); 
        }

        // PUT api/user/:id
        [HttpPut("{userId}")]
        public ActionResult<UserDb> EditUserInfo(Guid userId, [FromBody] UserDb info)
        {
            var user = _userContext.User.Find(userId);

            if (user == null)
            {
                return BadRequest(new {message = "ID does not exist"});
            }

            // _userContext.User.SetValue(info);
            foreach (var field in typeof (UserDb).GetProperties().Where(p => (p.GetValue(info) != null)))
            {
                if (!(field.PropertyType == typeof (DateTime) && field.GetValue(info).ToString() == new DateTime().ToString()))
                {
                    field.SetValue(user, field.GetValue(info));
                }
            }
            _userContext.SaveChanges();
            return Ok(user);
        }

        // POST api/userdb
        [HttpPost]
        public ActionResult<UserDb> AddUser([FromBody] UserDb info)
        {
            UserDb user = new UserDb { 
                FirstName = info.FirstName,
                Surname = info.Surname, 
                Email = info.Email,
                Role = info.Role,
                Location = info.Location,
                ManagerEmail = info.ManagerEmail,
                AdminLevel = info.AdminLevel,
                Salary = info.Salary,
                Password = info.Password
            };
            _userContext.User.Add(user);
            _userContext.SaveChanges();

            return Ok(user);
        }

        // DELETE api/userdb/:id
        [HttpDelete("{userId}")]
        // public ActionResult<UserDb> AddUser([FromBody] UserDb info)
        public ActionResult<string> RemoveUser(Guid userId)
        {
            var user = _userContext.User.Find(userId);
            if (user == null)
            {
                return BadRequest(new {message = "ID does not exist"});
            }

            _userContext.User.Remove(user);
            _userContext.SaveChanges();

            return Ok("User deleted");
        }
    }
}