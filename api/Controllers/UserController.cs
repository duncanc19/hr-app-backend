using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRApp.API.Models;
using System.Reflection;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using HRApp.API.Services;

namespace HRApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _userContext;
        public UserController(UserContext userContext)
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

        // GET api/user
        [HttpGet]
        public ActionResult<List<UserDb>> GetAll([FromHeader] string adminLevel, [FromHeader] string email)
        {
            if (adminLevel.Equals("Admin")) 
            {
                var allUsers = _userContext.User.ToList();
                return Ok(new{users = allUsers});
            } 
            else if (adminLevel == "Manager") 
            {
                var managees = _userContext.User.Where(user => (user.ManagerEmail == email)).ToList();
                return Ok(new{users = managees});
            } 
            return BadRequest(new {message = "You are not authorised to do this"});
        }

         // GET api/user/:id
        [HttpGet("{userId}")]
        public ActionResult<UserDb> GetUserById(Guid userId)
        {
            var user = _userContext.User.Find(userId);

            if (user == null) {
                return BadRequest(new {message = "ID does not exist"});
            }
            return Ok(new{user = user}); 
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
            
            foreach (var field in typeof (UserDb).GetProperties().Where(p => (p.GetValue(info) != null)))
            {
                if (field.Name == "Password")
                {
                    return Unauthorized(new {message = "You are not authorized to change the password from this endpoint"});
                } 
                else if (!(field.PropertyType == typeof (DateTime) && field.GetValue(info).ToString() == new DateTime().ToString()))
                {
                    field.SetValue(user, field.GetValue(info));
                }
            }
            _userContext.SaveChanges();
            return Ok(new{ user = user});
        }

        // PUT api/user/:id/password
        [HttpPut("{userId}/password")]
        public ActionResult<string> ChangePassword(Guid userId, [FromBody] ChangePassword passwordObj)
        {
            var user = _userContext.User.Find(userId);

            if (user == null)
            {
                return BadRequest(new {message = "User does not exist"});
            };
            string savedPasswordHash = user.Password; 
            bool oldPasswordMatches = PasswordHash.ValidPassword(passwordObj.OldPassword, savedPasswordHash);

            if (!oldPasswordMatches) 
            {
                return BadRequest(new {message = "Old password is incorrect"});
            };
            var newHashedPassword = PasswordHash.HashPassword(passwordObj.NewPassword);
            user.Password = newHashedPassword;
            _userContext.SaveChanges();
            return Ok(new {message = "Password was successfully changed"});
        }

        // POST api/user
        [HttpPost]
        public ActionResult<UserDb> AddUser([FromBody] UserDb info, [FromHeader] string adminLevel)
        {
            if (adminLevel == "Admin")
            {
                UserDb newUser = new UserDb { 
                    FirstName = info.FirstName,
                    Surname = info.Surname, 
                    Email = info.Email,
                    Role = info.Role,
                    Location = info.Location,
                    ManagerEmail = info.ManagerEmail,
                    AdminLevel = info.AdminLevel,
                    Salary = info.Salary,
                    Password = PasswordHash.HashPassword(info.Password)
                };

                _userContext.User.Add(newUser);
                _userContext.SaveChanges();
                return Ok(new{user = newUser});
            };
            return BadRequest(new {message = "You are not authorised to do this"});
        }

        // DELETE api/user/:id
        [HttpDelete("{userId}")]
        public ActionResult<string> RemoveUser(Guid userId, [FromHeader] string adminLevel)
        {
            if (adminLevel == "Admin")
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
            return BadRequest(new {message = "You are not authorised to do this"});
        }
    }
}