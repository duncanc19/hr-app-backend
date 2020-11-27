using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HRApp.API.Models;
using Microsoft.AspNetCore.Cors;

namespace HRApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class VisitorController : ControllerBase
    {
        private readonly VisitorContext _visitorContext;
        public VisitorController(VisitorContext visitorContext)
        {
            _visitorContext = visitorContext;
        }

        // GET api/visitor
        [HttpGet]
        public ActionResult<List<VisitorDb>> GetAll([FromHeader] string adminLevel, [FromHeader] string email)
        {
            var allVisitors = new List<VisitorDb>();
            if (adminLevel.Equals("Admin"))
            {
                allVisitors = _visitorContext.Visitor.ToList();
            }
            else 
            {
                allVisitors = _visitorContext.Visitor.Where(visitor => (visitor.EmployeeEmail == email)).ToList();
            }
            return Ok(new{visitors = allVisitors});
        }

        // POST api/visitor
        [HttpPost]
        public ActionResult<VisitorDb> AddVisit([FromBody] VisitorDb info)
        {
            VisitorDb newVisit = new VisitorDb { 
                FirstName = info.FirstName,
                Surname = info.Surname, 
                Company = info.Company,
                Role = info.Role,
                Telephone = info.Telephone,
                Email = info.Email,
                EmployeeEmail = info.EmployeeEmail,
                Appointment = info.Appointment,
            };
            _visitorContext.Visitor.Add(newVisit);
            _visitorContext.SaveChanges();

            return Ok(new{visit = newVisit});
        }

        // GET api/user/:id
        [HttpGet("{visitId}")]
        public ActionResult<VisitorDb> GetUserById(Guid visitId)
        {
            var visit = _visitorContext.Visitor.Find(visitId);

            if (visit == null) {
                return NotFound(new {message = "Visit does not exist"});
            }
            return Ok(new{visit = visit}); 
        }

        // DELETE api/visitor/:id
        [HttpDelete("{visitId}")]
        public ActionResult<string> RemoveUser(Guid visitId)
        {
            var visit = _visitorContext.Visitor.Find(visitId);
            if (visit == null)
            {
                return BadRequest(new {message = "Visit does not exist"});
            }
            _visitorContext.Visitor.Remove(visit);
            _visitorContext.SaveChanges();
            return Ok(new {message = "Visit has been deleted successfully" });
        }

        // PUT api/visit/:id
        [HttpPut("{visitId}")]
        public ActionResult<UserDb> EditVisitInfo(Guid visitId, [FromBody] VisitorDb info)
        {
            var visit = _visitorContext.Visitor.Find(visitId);

            if (visit == null)
            {
                return BadRequest(new {message = "ID does not exist"});
            }

            foreach (var field in typeof (VisitorDb).GetProperties().Where(p => (p.GetValue(info) != null)))
            {
                if (!(field.PropertyType == typeof (DateTime) && field.GetValue(info).ToString() == new DateTime().ToString()))
                {
                    field.SetValue(visit, field.GetValue(info));
                }
            }
            _visitorContext.SaveChanges();
            return Ok(new{ visit = visit});
        }
    }
}