using Microsoft.AspNetCore.Identity;

namespace ASP_NET_Core_Identity_at_HardWork.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Description { get; set; } = "Пустий опис користувача";
    }
}
