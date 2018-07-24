using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MirysList.Models;
using MirysList.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

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
            dbContext.Database.EnsureCreated();
        }
    }
}
