using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Description { get; set; } = "Пустий опис користувача";
    }
}
