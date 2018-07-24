using Microsoft.AspNetCore.Authorization;
using MirysList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Authorization
{
    public class ApprovedListerValidationHandler : AuthorizationHandler<ApprovedListerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApprovedListerRequirement requirement)
        {
            
        }
    }
}
