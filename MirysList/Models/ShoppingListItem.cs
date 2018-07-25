using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem()
        {

        }

        public ShoppingListItem(ShoppingListItem item)
        {
            this.Id = item.Id;
            this.CatalogItemId = item.CatalogItemId;
            this.ItemNotes = item.ItemNotes;
            this.Quantity = item.Quantity;
        }

        [Required]
        [Key]
        public int Id { get; set; }       
        
        public int CatalogItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string ItemNotes { get; set; }
    }

    public class UpdatedShoppingListItem : ShoppingListItem
    {
        public UpdatedShoppingListItem(ShoppingListItem item) :base(item)
        {

        }

        public CatalogItem CatalogItem { get; set; }
    }
}
