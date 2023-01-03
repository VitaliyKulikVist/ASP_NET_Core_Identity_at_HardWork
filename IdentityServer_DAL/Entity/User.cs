namespace IdentityServer_DAL.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
