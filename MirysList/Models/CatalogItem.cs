using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class CatalogItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string ImageUrl { get; set; }      
        public Category Category { get; set; }
       // public Catalog Catalog { get; set; }
        public string ItemNotes { get; set; }
    }
}
