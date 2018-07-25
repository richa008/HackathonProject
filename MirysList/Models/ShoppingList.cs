using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class ShoppingList
    {
        [Required]
        public int Id { get; set; }
        public Family Family { get; set; }
        public List<ShoppingListItem> listItems { get; set; }
    }
}
