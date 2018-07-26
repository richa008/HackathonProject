using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MirysList.Models;

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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            AddFamilySeedData(dbContext);
        }

        private void AddFamilySeedData(AppDbContext dbContext)
        {
            User user1 = new User { FirstName = "Shriya", LastName = "Aslamy", Gender = Gender.Female, BirthDate = new DateTime(1986, 4, 5), PhoneNumber = "425-345-3334", Language = "Urdu"};
            User user2 = new User { FirstName = "Amir", LastName = "Aslamy", Gender = Gender.Male, BirthDate = new DateTime(1985, 6, 20), PhoneNumber = "233-345-3334", Language = "Urdu"};
            User user3 = new User { FirstName = "Seema", LastName = "Sefat", Gender = Gender.Female, BirthDate = new DateTime(1999, 4, 5), PhoneNumber = "923-345-3334", Language = "Arabic" };
            User user4 = new User { FirstName = "Ali", LastName = "Sefat", Gender = Gender.Male, BirthDate = new DateTime(2008, 6, 21), PhoneNumber = "425-345-3234", Language = "Arabic" };
            User user5 = new User { FirstName = "Maya", LastName = "Sefat", Gender = Gender.Female, BirthDate = new DateTime(2015, 8, 9), PhoneNumber = "433-345-3234", Language = "Arabic" };
            User user6 = new User { FirstName = "Temur", LastName = "Sefat", Gender = Gender.Male, BirthDate = new DateTime(1998, 6, 20), PhoneNumber = "212-345-5344", Language = "Arabic" };
            
            dbContext.Users.AddRange(new List<User> { user1, user2, user3, user4, user5, user6 });

            Family family1 = new Family
            {
                FamilyName = "Aslamy",
                FamilyCareOf = "Miry",
                PhoneNumber = "425-345-3334",
                Story = "Family from Afghanistan arrived mid 2017 and settled in San Diego. Both husband and wife are civil engineers and previously worked with USAID in Afghanistan, but due to imminent danger they were forced to leave their country",
                StreetAddressLine1 = "32 Dennis Street",
                City = "San Diego",
                Country = "USA",
                PostalCode = "90210",
                FamilyMembers = new List<User> { user1, user2 }
            };

            Family family2 = new Family
            {
                FamilyName = "Sefat",
                FamilyCareOf = "Jones",
                PhoneNumber = "235-345-3334",
                Story = "The Sefat Family moved with 2 kids from Syria. Dad was mechanical driver and security guard. Due to being in danger they left their home country and were eventually resettled in Los Angeles. ",
                StreetAddressLine1 = "3234 Main Street",
                City = "Los Angeles",
                Country = "USA",
                PostalCode = "98304",
                FamilyMembers = new List<User> { user3, user4, user5, user6 }
            };

            Family family3 = new Family
            {
                FamilyName = "Noori",
                FamilyCareOf = "Jones",
                PhoneNumber = "235-233-2224",
                Story = "The Noori Family are from Afghanistan where they helped women and girls have access to education. They worked closely with American troops and were finally granted refugee status last year.",
                StreetAddressLine1 = "3234 Main Street",
                City = "Los Angeles",
                Country = "USA",
                PostalCode = "98304"
            };
            dbContext.Families.AddRange(new List<Family> { family1, family2, family3});
            dbContext.SaveChanges();

        }
    }
}
