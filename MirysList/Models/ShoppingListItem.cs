using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class ShoppingListItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public CatalogItem CatalogItem { get; set; }
        [Required]
        public int Quantity { get; set; }
      //  public ShoppingList ShoppingList { get; set; }
        //[Required]
       public string ItemNotes { get; set; }
    }
}
