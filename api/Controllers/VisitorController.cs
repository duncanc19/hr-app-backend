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
        public ActionResult<VisitorDb> AddVisitor([FromBody] VisitorDb info)
        {
            VisitorDb newVisitor = new VisitorDb { 
                FirstName = info.FirstName,
                Surname = info.Surname, 
                Company = info.Company,
                Role = info.Role,
                Telephone = info.Telephone,
                Email = info.Email,
                EmployeeEmail = info.EmployeeEmail,
                Appointment = info.Appointment,
            };
            _visitorContext.Visitor.Add(newVisitor);
            _visitorContext.SaveChanges();

            return Ok(new{visitor = newVisitor});
        }
    }
}