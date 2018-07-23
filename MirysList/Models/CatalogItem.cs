using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string ItemNotes { get; set; }
    }
}
