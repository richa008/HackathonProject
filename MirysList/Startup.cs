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
using Newtonsoft.Json;
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
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(Configuration["ConnectionString"]));
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
            AddSeedData(dbContext);
        }

        private void AddSeedData(AppDbContext dbContext)
        {
            Lister lister1 = new Lister { Id = "1687949164635588", FirstName = "Uday Kiran", LastName = "Ravuri" };
            Lister lister2 = new Lister { Id = "1687949164635587", FirstName = "Phil", LastName = "Rogan" };
            Lister lister3 = new Lister { Id = "1687949164635586", FirstName = "Richa", LastName = "Deshmukh" };
            Lister lister4 = new Lister { Id = "1687949164635585", FirstName = "Swapna", LastName = "Guddanti" };
            Lister lister5 = new Lister { Id = "1687949164635584", FirstName = "Ankur", LastName = "Gupta" };
            dbContext.Listers.AddRange(new List<Lister> { lister1, lister2, lister3, lister4, lister5 });

            Category beauty = new Category { Title = "Beauty and personal care", ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fblog.euromonitor.com%2Fwp-content%2Fuploads%2F2017%2F09%2FBeauty-Brushes-2.jpg&imgrefurl=https%3A%2F%2Fblog.euromonitor.com%2F2017%2F10%2Fasia-leader-global-skincare-sales-in-cosmetics-asia-2017.html&docid=sUZ5ed7dS0RcpM&tbnid=A2iMYmn2OKDfXM%3A&vet=10ahUKEwjnrpi5u7ncAhUGIDQIHeu_CWQQMwiPASg5MDk..i&w=1200&h=800&bih=584&biw=857&q=beauty%20and%20personal%20care&ved=0ahUKEwjnrpi5u7ncAhUGIDQIHeu_CWQQMwiPASg5MDk&iact=mrc&uact=8" };
            Category laundry = new Category { Title = "Laundry", ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Ffacilities.georgetown.edu%2Fsites%2Ffacilities%2Ffiles%2Fstyles%2Frotator_image_overlay_unit_profile%2Fpublic%2Flaundry_basket_0.jpg&imgrefurl=https%3A%2F%2Ffacilities.georgetown.edu%2FLaundry&docid=UozY7Fv9P23v8M&tbnid=Av53_BR8DIxs8M%3A&vet=10ahUKEwj0iaeFvLncAhWpGDQIHVklC2EQMwigAigDMAM..i&w=768&h=433&bih=584&biw=857&q=laundry&ved=0ahUKEwj0iaeFvLncAhWpGDQIHVklC2EQMwigAigDMAM&iact=mrc&uact=8" };
            Category furniture = new Category { Title = "Furniture", ImageUrl = "" };
            Category rugs = new Category { Title = "Rugs" };
            dbContext.Categories.AddRange(new List<Category> { beauty, laundry, furniture, rugs });

            CatalogItem liquidHandSoap = new CatalogItem { Title = "Liquid Hand Soap", Category = beauty, ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fwww.dollartree.com%2Fassets%2Fproduct_images_2016%2Fstyles%2Fxlarge%2F199810.jpg&imgrefurl=https%3A%2F%2Fwww.dollartree.com%2FLavender-Chamomile-Liquid-Hand-Soap-14-oz-%2Fp347354%2Findex.pro&docid=kQZyZRC9liRk-M&tbnid=Zvj6QV8GyM0KcM%3A&vet=10ahUKEwjDt4D0v7ncAhUzO30KHTNWAewQMwjvAigAMAA..i&w=480&h=480&bih=584&biw=857&q=liquid%20hand%20soap&ved=0ahUKEwjDt4D0v7ncAhUzO30KHTNWAewQMwjvAigAMAA&iact=mrc&uact=8", ItemNotes = "16 oz" };
            CatalogItem shampoo = new CatalogItem { Title = "Shampoo and Conditioner", Category = beauty };
            CatalogItem detergent = new CatalogItem { Title = "Laundry Detergent", Category = laundry, ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fimages-na.ssl-images-amazon.com%2Fimages%2FI%2F81fYpw6iY8L._SL1500_.jpg&imgrefurl=https%3A%2F%2Fwww.amazon.com%2FTide-Original-Laundry-Detergent-Packaging%2Fdp%2FB00IO970FC&docid=94ELvykyD2PbbM&tbnid=oMXsfGFfGO80CM%3A&vet=10ahUKEwjxg9OVwbncAhWSHjQIHao7CUMQMwjIASgAMAA..i&w=1500&h=1500&bih=689&biw=1371&q=laundry%20detergent&ved=0ahUKEwjxg9OVwbncAhWSHjQIHao7CUMQMwjIASgAMAA&iact=mrc&uact=8", ItemNotes = "50 fl Oz, Pack of 2" };
            CatalogItem futon = new CatalogItem { Title = "Futon Couch Bed", Category = furniture, ImageUrl = "https://www.google.com/aclk?sa=l&ai=DChcSEwiYqaCGwrncAhUR1GQKHaN4CD0YABAHGgJwag&sig=AOD64_2EgaehwThNc6FNQ0E727UBRku2UA&ctype=5&q=&ved=0ahUKEwjKmpyGwrncAhXTMX0KHZH7AFMQwg8IxgI&adurl=", ItemNotes = "Sleeper" };
            CatalogItem floorMats = new CatalogItem { Title = "Floor Mats", Category = rugs, ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2F3.imimg.com%2Fdata3%2FCW%2FPN%2FMY-2547025%2Fpvc-floor-mats-500x500.jpg&imgrefurl=https%3A%2F%2Fwww.indiamart.com%2Fproddetail%2Fpvc-cushion-floor-mats-5037729612.html&docid=wDdV4rf9ddVW_M&tbnid=5wnK-9Fg9CcJtM%3A&vet=10ahUKEwiNsZzIwrncAhXBIDQIHdzcBA0QMwiwAigIMAg..i&w=500&h=500&bih=689&biw=1371&q=Floor%20mats&ved=0ahUKEwiNsZzIwrncAhXBIDQIHdzcBA0QMwiwAigIMAg&iact=mrc&uact=8" };
            dbContext.CataLogItems.AddRange(new List<CatalogItem> { liquidHandSoap, shampoo, detergent, futon, floorMats });

            Catalog homeNeeds = new Catalog { Title = "Home Needs", CreatedDate = DateTime.UtcNow, Items = new List<CatalogItem> { liquidHandSoap, shampoo, detergent, futon, floorMats } };
            dbContext.Catalogs.Add(homeNeeds);

            FamilyMember f1Member = new FamilyMember { FirstName = "F1 M1 FN", LastName = "F1 M1 LN", Gender = Gender.Male, BirthDate = DateTime.Parse("01/01/2000"), Language = "Arabic" };
            FamilyMember f2Member = new FamilyMember { FirstName = "F2 M1 FN", LastName = "F2 M1 LN", Gender = Gender.Female, BirthDate = DateTime.Parse("12/12/1990"), Language = "Urdu" };
            dbContext.FamilyMembers.AddRange(new List<FamilyMember> { f1Member, f2Member });

            Family family1 = new Family { FamilyName = "Family1 Name", FamilyCareOf = "Family1 Head", City = "Sacramento", Country = "USA", StreetAddressLine1 = "12345 10th St NE", StreetAddressLine2 = "Apt #007", PhoneNumber = "123-123-1234", PostalCode = "12345", PhotoUrl = "", Story = "Family1's background story" };
            Family family2 = new Family { FamilyName = "Family2 Name", FamilyCareOf = "Family2 Head", City = "El Cajon", Country = "USA", StreetAddressLine1 = "54321 30th St NW", StreetAddressLine2 = "Apt #321", PhoneNumber = "123-123-1234", PostalCode = "54321", PhotoUrl = "", Story = "Family2's background story" };
            List<Family> families = new List<Family> { family1, family2 };
            dbContext.Families.AddRange(families);
            dbContext.SaveChanges();

            //EntityEntry<Family> existingFamily1 = dbContext.Families.Attach(family1);

            family1.FamilyMembers = new List<FamilyMember> { f1Member };
            family2.FamilyMembers = new List<FamilyMember> { f2Member };
            dbContext.Families.UpdateRange(new List<Family> { family1, family2 });

            // TODO: Include ShoppingListItems here.

            dbContext.SaveChanges();
        }
    }
}
