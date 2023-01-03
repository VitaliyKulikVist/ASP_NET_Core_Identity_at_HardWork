using IdentityServer_DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer_DAL.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
