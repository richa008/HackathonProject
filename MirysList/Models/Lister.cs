using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class Lister
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        public string Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<Family> Families { get; set; }
    }
}
