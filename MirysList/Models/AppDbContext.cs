using System;
using System.Collections.Generic;
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
        public DbSet<List> ShoppingLists { get; set; }
        public DbSet<ListItem> ShoppingListItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
