using Microsoft.AspNetCore.Identity;

namespace FinalDern_Support.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string type { get; set; }

        // Navigation properties
        public Admin Admin { get; set; }
        public Customer Customer { get; set; }
        public Technician Technician { get; set; }
    }
}
