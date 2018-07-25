using System.Security.Claims;
using System.Security.Principal;

namespace MirysList.Models
{
    public class Principal : ClaimsPrincipal
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name {
            get
            {
                return $"{this.FirstName}{this.LastName}";
            }
        }
    }
}
