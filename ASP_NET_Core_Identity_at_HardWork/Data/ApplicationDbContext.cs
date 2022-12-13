using ASP_NET_Core_Identity_at_HardWork.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_Core_Identity_at_HardWork.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Налаштуйте модель ASP.NET Identity і замініть значення за замовчуванням, якщо потрібно.
            // Наприклад, ви можете перейменувати імена таблиць ASP.NET Identity тощо.
            // Додайте свої налаштування після виклику base.OnModelCreating(builder);
        }
    }
}
