namespace IdentityServer
{
    public class ConstantIdentityServer
    {
        public static string ConnectionMySQL { get; } = "server=localhost;user=vitaliy;password=12345678;database=TestConnectionName;";

        public static string CommandSeeding { get; } = "/seed";

        public static string CommandDeleting { get; } = "/delete";
    }
}
