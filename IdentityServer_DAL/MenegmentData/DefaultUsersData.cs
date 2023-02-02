using System.Collections.Generic;

namespace IdentityServer_DAL.MenegmentData
{
    public class DefaultUsersData
    {
        public static Dictionary<string, string> UsersDictionary { get; } = new Dictionary<string, string>()
        {
            {
                "alica","Pass123$"
            },
            {
                "bobik","Pass123$"
            },
            {
                "Vitaliy","Qwe123^&*"
            }
        };
    }
}
