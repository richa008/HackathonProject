using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class Family
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string FamilyName { get; set; }
        public string Phone { get; set; }
        public string Story { get; set; }
        [Required]
        public string StreetAdresss1 { get; set; }
        public string StreetAdresss2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
