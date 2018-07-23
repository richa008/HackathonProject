using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class Family
    {
        public long Id { get; set; }
        public string FamilyName { get; set; }
        public string Phone { get; set; }
        public string Story { get; set; }
        public string StreetAdresss1 { get; set; }
        public string StreetAdresss2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
