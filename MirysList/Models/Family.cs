using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirysList.Models
{
    public class Family
    {
        public long Id { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string FamilyCareOf { get; set; }

        public string Phone { get; set; }

        public string Story { get; set; }

        [Required]
        public string StreetAdresssLine1 { get; set; }

        public string StreetAdresssLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public string PhotoUrl { get; set; }

        public virtual ICollection<User> FamilyMembers {get; set;}
    }
}
