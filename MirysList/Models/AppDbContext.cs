using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MirysList.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Family> Families { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<CatalogItem> CataLogItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
    }
}
