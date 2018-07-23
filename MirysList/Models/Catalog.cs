using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class Catalog
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Title { get; set; }
        public List<CatalogItem> Items { get; set; }
        public List<Category> Categories { get; set; }
    }
}
