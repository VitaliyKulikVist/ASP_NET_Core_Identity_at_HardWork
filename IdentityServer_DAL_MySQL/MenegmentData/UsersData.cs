using System.Collections.Generic;

namespace IdentityServer_DAL_MySQL.MenegmentData
{
    public class UsersData
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
