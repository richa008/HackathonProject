﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MirysList.Controllers;
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
            IQueryable<UserRole> listerIds = _dbContext.UserRoles.Include(p => p.Id).Where(role => role.Role == Role.Lister);
            return Task.CompletedTask;
        }
    }
}
