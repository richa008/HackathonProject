using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MirysList.Models;
using MirysList.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace MirysList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DbContextConnectionString")));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("FbLogin", policy => {
                    policy.Requirements.Add(new FbLoginRequirement());
                });
                options.AddPolicy("ApprovedLister", policy =>
                {
                    policy.Requirements.Add(new ApprovedListerRequirement());
                });
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = async context =>
                    {
                        context.Response.StatusCode = 401;
                        ErrorMessage error = new ErrorMessage("A valid authorization header is required.");
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(error)).ConfigureAwait(false);
                    };
                });

            services.AddSingleton<IAuthorizationHandler, FbLoginValidationHandler>();
            services.AddTransient<IAuthorizationHandler, ApprovedListerValidationHandler>();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 44337;
            });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                ErrorMessage error = new ErrorMessage("Unknown error");
                switch (context.HttpContext.Response.StatusCode)
                    {
                        case 403:
                            {
                                error = new ErrorMessage("User not authorized to access this API.");
                                break;
                            }

                        case 404:
                            {
                                error = new ErrorMessage("Path not found.");
                                break;
                            }

                        case 500:
                            {
                                error = new ErrorMessage("Internal server error.");
                                break;
                            }
                    }

                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(error)).ConfigureAwait(false);
            });

            app.UseMvc();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // add in seed data to database
            // AddSeedData(dbContext);
        }

        private void AddSeedData(AppDbContext dbContext)
        {
            User user1 = new User { Id = 1687949164635588, FirstName = "Lister", LastName = "Uday", Gender = Gender.Male, Language = "English", PhoneNumber = "123-234-3456", BirthDate = DateTime.Parse("11/11/1994") };
            User user2 = new User { Id = 1687949164635587, FirstName = "Lister", LastName = "Phil", Gender = Gender.Male, Language = "English", PhoneNumber = "123-234-3456", BirthDate = DateTime.Parse("11/11/1994") };
            User user3 = new User { Id = 1687949164635586, FirstName = "Lister", LastName = "Richa", Gender = Gender.Female, Language = "English", PhoneNumber = "123-234-3456", BirthDate = DateTime.Parse("11/11/1994") };
            User user4 = new User { Id = 1687949164635585, FirstName = "Lister", LastName = "Swapna", Gender = Gender.Female, Language = "English", PhoneNumber = "123-234-3456", BirthDate = DateTime.Parse("11/11/1994") };
            User user5 = new User { Id = 1687949164635584, FirstName = "Lister", LastName = "Ankur", Gender = Gender.Male, Language = "English", PhoneNumber = "123-234-3456", BirthDate = DateTime.Parse("11/11/1994") };
            dbContext.Users.AddRange(new List<User> { user1, user2, user3, user4, user5 });
            dbContext.SaveChanges();

            List<UserRole> userRoles = new List<UserRole>();
            userRoles.Add(new UserRole { Id = 1687949164635588, Role = Role.Lister, User = user1 });
            userRoles.Add(new UserRole { Id = 1687949164635587, Role = Role.Lister, User = user2 });
            userRoles.Add(new UserRole { Id = 1687949164635586, Role = Role.Lister, User = user3 });
            userRoles.Add(new UserRole { Id = 1687949164635585, Role = Role.Lister, User = user4 });
            userRoles.Add(new UserRole { Id = 1687949164635584, Role = Role.Lister, User = user5 });
            dbContext.UserRoles.AddRange(userRoles);
            dbContext.SaveChanges();

            //List<Category>

            dbContext.SaveChanges();
        }
    }
}
