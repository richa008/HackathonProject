using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MirysList.Models
{ 
    public class User
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public String FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public String LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string PhotoUrl { get; set; }

        public string Language { get; set; }

        public Family Family { get; set; }
    }
}
