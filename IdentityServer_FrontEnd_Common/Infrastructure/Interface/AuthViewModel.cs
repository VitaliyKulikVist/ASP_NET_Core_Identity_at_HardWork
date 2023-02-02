namespace IdentityServer_FrontEnd_Common.Infrastructure.Interface
{
    public  interface IAuthViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
