using IdentityServer_DAL.Data;

namespace IdentityServer_DAL.Configurations
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
