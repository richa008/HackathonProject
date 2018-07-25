using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class Catalog
    {
        public int Id { get; set; }

        [Required]        
        public DateTime CreatedDate { get; set; }    
        
        public DateTime ModifiedDate { get; set; }
        
        public string Title { get; set; }

        public virtual ICollection<CatalogItem> Items { get; set; }
    }
}
