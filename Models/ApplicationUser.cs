using Microsoft.AspNetCore.Identity;

namespace IT15_Trojan_B.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        
    }
}
