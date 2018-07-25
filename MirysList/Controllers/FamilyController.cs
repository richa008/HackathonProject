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
        [HttpGet]
        [Route("families/listerId/{id}")]
        public IActionResult GetFamilies(int id)
        {
            var lister = _dbContext.Lister.Include(p => p.Families).SingleOrDefault(m => m.Id == id);
            if (lister == null)
            {
                return NotFound("No record found with this Id");
            }
            var families = _dbContext.Families.OrderBy(p => p.FamilyName);
            return Ok(families);
        }

        // GET: family/5
        [HttpGet]
        [Route("families/{id}")]
        public IActionResult GetFamily(int id)
        {
            var family = _dbContext.Families.Include(p => p.FamilyMembers).Include(i => i.ListItems).SingleOrDefault(m => m.Id == id);
            if (family == null)
            {
                return NotFound("No record found with this Id");
            }
       
            return Ok(family);
        }

        // POST: family
        [HttpPost]
        [Route("family/listerId/{id}")]
        public IActionResult Post(int id, [FromBody]Family family)
        {
            var lister = _dbContext.Lister.Include(p => p.Families).SingleOrDefault(m => m.Id == id);
            if (lister == null)
            {
                return NotFound("No record found with this Id");
            }
            try
            {
                lister.Families.Add(family);
                _dbContext.Lister.Update(lister);
                _dbContext.Families.Add(family);
                if (family.FamilyMembers != null)
                {
                    foreach (FamilyMember member in family.FamilyMembers)
                    {
                        _dbContext.FamilyMembers.Add(member);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("Error while saving record" + e);
            }
            return Ok(family);
        }

        // PUT: family/2
        // Doesn't update family members and list items
        [HttpPut]
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
            if (family.FamilyMembers.Count > 0 || family.ListItems.Count > 0)
            {
                return BadRequest("Family members and list items in request should be 0");
            }
            try
            {
                _dbContext.Families.Update(family); 
                _dbContext.SaveChanges(true);
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return NotFound("Error while udating record" + e);
            }
           
            return Ok(family);
        }
    }
}
