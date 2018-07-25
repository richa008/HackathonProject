using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MirysList.Models;
using Microsoft.EntityFrameworkCore;

namespace MirysList.Controllers
{
    [Authorize(Policy = "FbLogin")]
    [Authorize(Policy = "ApprovedLister")]
    [Produces("application/json")]
    public class FamilyController : Controller
    {
        private AppDbContext _dbContext;

        public FamilyController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: families
        [Route("families")]
        public IActionResult GetFamilies(string sort)
        {
            IQueryable<Family> families;
            switch(sort)
            {
                case "desc":
                    families = _dbContext.Families.Include(p => p.FamilyMembers).OrderByDescending(p => p.FamilyName);
                    break;
                default:
                    families = _dbContext.Families.Include(p => p.FamilyMembers).OrderBy(p => p.FamilyName);
                    break;
            }
            return Ok(families);
        }

        // GET: family/5
        [Route("families/{id}")]
        public IActionResult GetFamily(int id)
        {
            var family = _dbContext.Families.Include(p => p.FamilyMembers).SingleOrDefault(m => m.Id == id);
            if (family == null)
            {
                return NotFound("No record found with this Id");
            }
       
            return Ok(family);
        }

        // POST: family
        [Route("family")]
        public IActionResult Post([FromBody]Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dbContext.Families.Add(family);

                if (family.FamilyMembers != null)
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("Error while saving record" + e);
            }

            return Ok(family);
        }

        // PUT: family/2
        [Route("family/{id}")]
        public IActionResult Put(int id, [FromBody]Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != family.Id)
            {
                return BadRequest("Query parameter Id and the Id on the input record are not the same");
            }
            try
            {
                var originalFamily = _dbContext.Families.Include(p => p.FamilyMembers).SingleOrDefault(p => p.Id == id);
                foreach (var member in family.FamilyMembers)
                {
                    if (!originalFamily.FamilyMembers.Any(item => item.Id == member.Id))
                    {
                        return BadRequest("Family member id is not valid");
                    }
                    _dbContext.Users.Update(member);
                }
                _dbContext.Families.Update(family); 
                _dbContext.SaveChanges(true);
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return NotFound("Error while udating record" + e);
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
