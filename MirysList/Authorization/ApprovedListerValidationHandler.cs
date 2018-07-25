using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using MirysList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Authorization
{
    public class ApprovedListerValidationHandler : AuthorizationHandler<ApprovedListerRequirement>
    {
        private AppDbContext _dbContext;

        public ApprovedListerValidationHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApprovedListerRequirement requirement)
        {
            IQueryable<string> listerIds = _dbContext.Listers.Select(lister => lister.Id);

            if (context.Resource is AuthorizationFilterContext mvc)
            {
                if (mvc.HttpContext.User != null)
                {
                    // retrieve Id from the user principal and compare it against the list of approved listerIds.
                    Principal userPrincipal = (Principal)mvc.HttpContext.User;
                    HashSet<string> listerIdSet = new HashSet<string>();
                    foreach (string id in listerIds)
                    {
                        listerIdSet.Add(id);
                    }

                    if (listerIdSet.Contains(userPrincipal.Id))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
                
                context.Fail();
                return Task.CompletedTask;   
            }
            else
            {
                throw new Exception("MVC context does not exist");
            }
        }
    }
}
