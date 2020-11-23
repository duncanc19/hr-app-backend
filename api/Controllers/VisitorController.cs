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
    public class VisitorController : ControllerBase
    {
        private readonly VisitorContext _visitorContext;
        public VisitorController(VisitorContext visitorContext)
        {
            _visitorContext = visitorContext;
        }

        // GET api/visitor
        [HttpGet]
        public ActionResult<List<VisitorDb>> GetAll()
        {
            var allVisitors = _visitorContext.Visitor.ToList();
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
    }
}