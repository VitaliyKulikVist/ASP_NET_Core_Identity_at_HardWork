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
                "bob","Pass123$"
            },
            {
                "Vitaliy","Qwerty12345678!@#$%^&*"
            }
        };
    }
}
