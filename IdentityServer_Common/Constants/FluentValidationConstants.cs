namespace IdentityServer_Common.Constants
{
    public static class FluentValidationConstants
    {
        public static int MinimumLengthUserName { get; } = 4;

        public static int MaximumLengthUserName { get; } = 10;

        public static int MinimumLengthPassword { get; } = 4;

        public static int MaximumLengthPassword { get; } = 24;

        public static string PasswordRegEx { get; } = "^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$";

    }
}
