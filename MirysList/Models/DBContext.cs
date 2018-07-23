using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MirysList.Models
{
	public class MirysListDBContext : DbContext
    {

        public MirysListDBContext(DbContextOptions<MirysListDBContext> options) : base(options)
        {
        }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<CatalogItem> CataLogItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<List> ShoppingLists { get; set; }
        public DbSet<ListItem> ShoppingListItems { get; set; }
	}
}
