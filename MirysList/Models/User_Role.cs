using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Not used for Hackathon
namespace MirysList.Models
{
    public class UserRole
    {
        public long Id { get; set; }

        public Role Role { get; set; }

        public virtual FamilyMember User { get; set; }
    }
}
