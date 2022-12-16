using IdentityServer.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
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
