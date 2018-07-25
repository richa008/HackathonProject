using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MirysList.Models
{ 
    public class FamilyMember
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

        [DataType(DataType.Date), Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PhotoUrl { get; set; }

        [StringLength(50)]
        public string Language { get; set; }
    }
}
