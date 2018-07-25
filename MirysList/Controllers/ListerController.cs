using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MirysList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Controllers
{
    [Authorize(Policy = "FbLogin")]
    [Authorize(Policy = "ApprovedLister")]
    [Produces("application/json")]
    public class ListerController : Controller
    {
        private AppDbContext _dbContext;

        public ListerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: families
        [Route("listers")]
        public IActionResult GetListers()
        {
            IQueryable<UserRole> listerIds = _dbContext.UserRoles.Include(p => p.Id).Where(role => role.Role == Role.Lister);
            
            return Ok(listerIds);
        }
    }
}
