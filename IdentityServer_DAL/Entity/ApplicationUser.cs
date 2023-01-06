using Microsoft.AspNetCore.Identity;

namespace IdentityServer_DAL.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string Description { get; set; } = "Пустий опис користувача";
    }
}
