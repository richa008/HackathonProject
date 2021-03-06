﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class Family
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FamilyName { get; set; }

        [Required]
        [StringLength(100)]
        public string FamilyCareOf { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string Story { get; set; }

        [Required]
        public string StreetAddressLine1 { get; set; }

        public string StreetAddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PhotoUrl { get; set; }

        public virtual ICollection<FamilyMember> FamilyMembers {get; set;}

        public virtual List<ShoppingListItem> ListItems { get; set; }
    }
}
