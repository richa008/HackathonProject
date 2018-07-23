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
    [Route("api/family_member")]
    public class FamilyMemberController : Controller
    {
        private AppDbContext _dbContext;

        public FamilyMemberController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/family_member/2
        [HttpGet("familyId")]
        public IActionResult Get(int familyId, string sort)
        {
           return Ok();
        }

        //// GET: api/family_member/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/family_member
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/family_member/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
    }
}
