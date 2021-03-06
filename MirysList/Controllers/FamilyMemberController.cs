﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MirysList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MirysList.Controllers
{
    [Authorize(Policy = "FbLogin")]
    [Authorize(Policy = "ApprovedLister")]
    [Produces("application/json")]
    public class FamilyMemberController : Controller
    {
        private AppDbContext _dbContext;

        public FamilyMemberController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: family_members/familyId/2
        [Route("family_members/familyId/{id}")]
        public IActionResult GetFamilyMembersByFamilyId(int id)
        {
            var family = _dbContext.Families.Include(p => p.FamilyMembers).SingleOrDefault(m => m.Id == id);
            if (family == null)
            {
                return NotFound("No record found with this Id");
            }

            return Ok(family.FamilyMembers);
        }

        // GET: family_member/5
        [Route("family_members/{id}")]
        public IActionResult GetFamilyMember(int id)
        {
            var members = _dbContext.FamilyMembers.Where(m => m.Id == id);
            if (members == null)
            {
                return NotFound("No record found with this Id");
            }

            return Ok(members);
        }

        // POST: family_member/familyId/2
        [Route("family_member/familyId/{id}")]
        public IActionResult Post(int id, [FromBody]FamilyMember user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var family = _dbContext.Families.Include(p => p.FamilyMembers).SingleOrDefault(m => m.Id == id);
            if (family == null)
            {
                return NotFound("No record found with this Id");
            }
            try
            {
                family.FamilyMembers.Add(user);
                _dbContext.Families.Update(family);
                _dbContext.FamilyMembers.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("Error while saving record" + e);
            }
            return Ok(user);
        }

        // PUT: family_member/5
        [Route("family_member/{id}")]
        public IActionResult Put(int id, [FromBody]FamilyMember user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != user.Id)
            {
                return BadRequest("Query parameter Id and the Id on the input record are not the same");
            }
            try
            {
                _dbContext.FamilyMembers.Update(user);
                _dbContext.SaveChanges(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("Error while udating record" + e);
            }

            return Ok(user);
        }
    }
}
