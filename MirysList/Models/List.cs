using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class List
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public List<ListItem> listItems { get; set; }
    }
}
