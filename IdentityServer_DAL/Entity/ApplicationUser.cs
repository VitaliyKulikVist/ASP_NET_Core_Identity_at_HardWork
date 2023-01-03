using Microsoft.AspNetCore.Identity;

namespace IdentityServer_DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Description { get; set; } = "Пустий опис користувача";
    }
}
