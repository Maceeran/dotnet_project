using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Offer>? Offers { get; set; }
    }
}
