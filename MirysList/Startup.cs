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

            // add in seed data to database 
            AddSeedData(dbContext);
        }

        private static void AddSeedData(AppDbContext dbContext)
        {
            // filling in demo categories
            Category beauty = new Category { Title = "Beauty and personal care", ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fblog.euromonitor.com%2Fwp-content%2Fuploads%2F2017%2F09%2FBeauty-Brushes-2.jpg&imgrefurl=https%3A%2F%2Fblog.euromonitor.com%2F2017%2F10%2Fasia-leader-global-skincare-sales-in-cosmetics-asia-2017.html&docid=sUZ5ed7dS0RcpM&tbnid=A2iMYmn2OKDfXM%3A&vet=10ahUKEwjnrpi5u7ncAhUGIDQIHeu_CWQQMwiPASg5MDk..i&w=1200&h=800&bih=584&biw=857&q=beauty%20and%20personal%20care&ved=0ahUKEwjnrpi5u7ncAhUGIDQIHeu_CWQQMwiPASg5MDk&iact=mrc&uact=8" };
            Category laundry = new Category { Title = "Laundry", ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Ffacilities.georgetown.edu%2Fsites%2Ffacilities%2Ffiles%2Fstyles%2Frotator_image_overlay_unit_profile%2Fpublic%2Flaundry_basket_0.jpg&imgrefurl=https%3A%2F%2Ffacilities.georgetown.edu%2FLaundry&docid=UozY7Fv9P23v8M&tbnid=Av53_BR8DIxs8M%3A&vet=10ahUKEwj0iaeFvLncAhWpGDQIHVklC2EQMwigAigDMAM..i&w=768&h=433&bih=584&biw=857&q=laundry&ved=0ahUKEwj0iaeFvLncAhWpGDQIHVklC2EQMwigAigDMAM&iact=mrc&uact=8" };
            Category furniture = new Category { Title = "Furniture", ImageUrl = "https://www.google.com/imgres?imgurl=http%3A%2F%2Fwww.foliot.com%2Fsites%2Fdefault%2Ffiles%2Ffoliot-furniture_adirondack-bedroom_940x400.jpg&imgrefurl=http%3A%2F%2Fwww.foliot.com%2F&docid=K1SIr4dSMRIUuM&tbnid=FBZF9O5Hb-_RSM%3A&vet=10ahUKEwjWv9PJrLvcAhUlKX0KHUkOAVAQMwiDAygQMBA..i&w=940&h=400&bih=689&biw=1371&q=furniture&ved=0ahUKEwjWv9PJrLvcAhUlKX0KHUkOAVAQMwiDAygQMBA&iact=mrc&uact=8" };
            Category rugs = new Category { Title = "Rugs", ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fcdn.shadesoflight.com%2Fmedia%2FW1siZiIsIjIwMTcvMTIvMDcvMTYvMTcvMTUvNjIvWFIxMzA1NC5qcGciXSxbInAiLCJvcHRpbSJdXQ%2FXR13054.jpg%3Fsha%3D53aa341853482e50&imgrefurl=https%3A%2F%2Fwww.shadesoflight.com%2Fpages%2Frugs&docid=fOpOCwEQ79rdKM&tbnid=1PoH9x1BlFBbHM%3A&vet=10ahUKEwjpvrPgrLvcAhXIITQIHWjUDYIQMwjeAigOMA4..i&w=609&h=427&bih=689&biw=1371&q=rugs&ved=0ahUKEwjpvrPgrLvcAhXIITQIHWjUDYIQMwjeAigOMA4&iact=mrc&uact=8" };
            
            dbContext.Categories.AddRange(new List<Category> { beauty, laundry, furniture, rugs });

            // filling in demo catalog items
            CatalogItem liquidHandSoap = new CatalogItem { Title = "Liquid Hand Soap", Category = beauty, ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fwww.dollartree.com%2Fassets%2Fproduct_images_2016%2Fstyles%2Fxlarge%2F199810.jpg&imgrefurl=https%3A%2F%2Fwww.dollartree.com%2FLavender-Chamomile-Liquid-Hand-Soap-14-oz-%2Fp347354%2Findex.pro&docid=kQZyZRC9liRk-M&tbnid=Zvj6QV8GyM0KcM%3A&vet=10ahUKEwjDt4D0v7ncAhUzO30KHTNWAewQMwjvAigAMAA..i&w=480&h=480&bih=584&biw=857&q=liquid%20hand%20soap&ved=0ahUKEwjDt4D0v7ncAhUzO30KHTNWAewQMwjvAigAMAA&iact=mrc&uact=8", ItemNotes = "16 oz" };
            CatalogItem shampoo = new CatalogItem { Title = "Shampoo and Conditioner", Category = beauty };
            CatalogItem detergent = new CatalogItem { Title = "Laundry Detergent", Category = laundry, ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fimages-na.ssl-images-amazon.com%2Fimages%2FI%2F81fYpw6iY8L._SL1500_.jpg&imgrefurl=https%3A%2F%2Fwww.amazon.com%2FTide-Original-Laundry-Detergent-Packaging%2Fdp%2FB00IO970FC&docid=94ELvykyD2PbbM&tbnid=oMXsfGFfGO80CM%3A&vet=10ahUKEwjxg9OVwbncAhWSHjQIHao7CUMQMwjIASgAMAA..i&w=1500&h=1500&bih=689&biw=1371&q=laundry%20detergent&ved=0ahUKEwjxg9OVwbncAhWSHjQIHao7CUMQMwjIASgAMAA&iact=mrc&uact=8", ItemNotes = "50 fl Oz, Pack of 2" };
            CatalogItem futon = new CatalogItem { Title = "Futon Couch Bed", Category = furniture, ImageUrl = "https://www.google.com/aclk?sa=l&ai=DChcSEwiYqaCGwrncAhUR1GQKHaN4CD0YABAHGgJwag&sig=AOD64_2EgaehwThNc6FNQ0E727UBRku2UA&ctype=5&q=&ved=0ahUKEwjKmpyGwrncAhXTMX0KHZH7AFMQwg8IxgI&adurl=", ItemNotes = "Sleeper" };
            CatalogItem floorMats = new CatalogItem { Title = "Floor Mats", Category = rugs, ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2F3.imimg.com%2Fdata3%2FCW%2FPN%2FMY-2547025%2Fpvc-floor-mats-500x500.jpg&imgrefurl=https%3A%2F%2Fwww.indiamart.com%2Fproddetail%2Fpvc-cushion-floor-mats-5037729612.html&docid=wDdV4rf9ddVW_M&tbnid=5wnK-9Fg9CcJtM%3A&vet=10ahUKEwiNsZzIwrncAhXBIDQIHdzcBA0QMwiwAigIMAg..i&w=500&h=500&bih=689&biw=1371&q=Floor%20mats&ved=0ahUKEwiNsZzIwrncAhXBIDQIHdzcBA0QMwiwAigIMAg&iact=mrc&uact=8" };

            dbContext.CataLogItems.AddRange(new List<CatalogItem> { liquidHandSoap, shampoo, detergent, futon, floorMats });

            // creating demo catalogs
            Catalog homeNeeds = new Catalog { Title = "Home Needs", CreatedDate = DateTime.UtcNow, Items = new List<CatalogItem> { liquidHandSoap, shampoo, detergent, futon, floorMats } };

            dbContext.Catalogs.Add(homeNeeds);

            dbContext.SaveChanges();
        }
    }
}
