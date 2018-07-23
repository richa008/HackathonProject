using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MirysList.Models;

namespace MirysList.Controllers
{
    [Produces("application/json")]
    [Route("api/family")]
    public class FamilyController : Controller
    {
        private AppDbContext _dbContext;

        public FamilyController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/family
        [HttpGet]
        public IActionResult Get(string sort)
        {
            IQueryable<Family> families;
            switch(sort)
            {
                case "desc":
                    families = _dbContext.Families.OrderByDescending(p => p.FamilyName);
                    break;
                default:
                    families = _dbContext.Families.OrderBy(p => p.FamilyName);
                    break;
            }
            return Ok(families);
        }

        // GET: api/family/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var family = _dbContext.Families.SingleOrDefault(m => m.Id == id);
            if (family == null)
            {
                return NotFound("No record found with this Id");
            }
       
            return Ok(family);
        }

        // POST: api/family
        [HttpPost]
        public IActionResult Post([FromBody]Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dbContext.Families.Add(family);

            if (family.FamilyMembers != null )
            {
                foreach (User user in family.FamilyMembers)
                {
                    _dbContext.Users.Add(user);
                    UserRole userRole = new UserRole();
                    userRole.User = user;
                    userRole.Role = Role.Family_Member;
                    _dbContext.UserRoles.Add(userRole);
                }
            }

            _dbContext.SaveChanges(true);
            return Ok(family);
        }

        // PUT: api/family/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != family.Id)
            {
                return BadRequest("Ids don't match");
            }
            try
            {
                _dbContext.Families.Update(family);
  
                _dbContext.SaveChanges(true);
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return NotFound("No record found with this Id" + e);
            }
           
            return Ok(family);
        }

        //// DELETE: api/Family/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var family = _dbContext.Families.SingleOrDefault(m => m.Id == id);
        //    if (family == null)
        //    {
        //        return NotFound("No record found with this Id");
        //    }
        //    try
        //    {
        //        _dbContext.Families.Remove(family);
        //        _dbContext.SaveChanges(true);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return BadRequest();
        //    }
        //    return Ok();
        //}

    }
}
