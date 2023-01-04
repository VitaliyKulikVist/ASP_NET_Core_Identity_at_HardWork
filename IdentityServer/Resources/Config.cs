using IdentityModel;
using IdentityServer_Common;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Resources
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResource(
                        name: "openid",
                        userClaims: new[] { "sub" },
                        displayName: "Your user identifier"),

                        new IdentityResource(
                        name: "profile",
                        userClaims: new[] { "name", "email", "website" },
                        displayName: "Your profile data"),

                        new IdentityResource()
                        {
                            //Вказує, чи цей ресурс увімкнено та чи можна його запитувати.(default: true)
                            Enabled = true,
                            //Унікальна назва ідентифікаційного ресурсу.
                            Name = "custom.profile",
                            //Це значення використовуватиметься, наприклад, на екрані згоди.
                            DisplayName = "Custom profile",
                            //Це значення використовуватиметься, наприклад, на екрані згоди.
                            Description = "It`s tests profile",
                            //Указує, чи може користувач скасувати вибір області на екрані згоди
                            //(якщо екран згоди хоче реалізувати таку функцію). За замовчуванням значення false.
                            Required = false,
                            //Указує, чи підкреслюватиме цю область на екрані згоди
                            //(якщо на екрані згоди потрібно реалізувати таку функцію).
                            //Використовуйте це налаштування для конфіденційних або важливих областей.
                            //За замовчуванням значення false.
                            Emphasize = false,
                            //Визначає, чи відображається ця область у документі відкриття.
                            //За замовчуванням значення true.
                            ShowInDiscoveryDocument = true,
                            //Список пов’язаних типів претензій користувачів,
                            //які мають бути включені в маркер ідентифікації.
                            UserClaims = new[] { "name", "email", "status" }
                        }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(IdentityServerScopeConstants.ApiScope_Level1, $"My {IdentityServerScopeConstants.ApiScope_Level1}"),
                new ApiScope(IdentityServerScopeConstants.ApiScope_Level2, $"My {IdentityServerScopeConstants.ApiScope_Level2}"),

                new ApiScope(name: IdentityServerScopeConstants.ApiScope_Read,   displayName: "Read your data."),
                new ApiScope(name: IdentityServerScopeConstants.ApiScope_Write,  displayName: "Write your data."),
                new ApiScope(name: IdentityServerScopeConstants.ApiScope_Delete, displayName: "Delete your data.")
            };

        public static IEnumerable<ApiResource> APIResource =>
            new ApiResource[]
            {
                new ApiResource("invoice", "Invoice API")
                {
                    Scopes = {
                        IdentityServerScopeConstants.ApiScope_Read,
                        IdentityServerScopeConstants.ApiScope_Write,
                        IdentityServerScopeConstants.ApiScope_Delete }
                },

                new ApiResource("customer", "Customer API")
                {
                    Scopes = {
                        IdentityServerScopeConstants.ApiScope_Level2 }
                },

                new ApiResource
                {
                    //Вказує, чи цей ресурс увімкнено та чи можна його запитувати. За замовчуванням значення true.
                    Enabled = true,
                    //Унікальна назва API. Це значення використовується для автентифікації з самоаналізом і буде додано до аудиторії вихідного маркера доступу.
                    Name = "castomResource",
                    //Це значення можна використовувати, наприклад, на екрані згоди.
                    DisplayName = "Test castom resource",
                    //Це значення можна використовувати, наприклад, на екрані згоди.
                    Description = "It`s a test API resource at castom resource",
                    //Секрет API використовується для кінцевої точки самоаналізу. API може автентифікуватися за допомогою інтроспекції за допомогою імені та секрету API.
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //Список пов’язаних типів претензій користувачів, які слід включити в маркер доступу.
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email },
                    //API повинен мати принаймні одну область дії. Кожен діапазон може мати різні налаштування.
                    Scopes = new[] { "castomResource.full_access", "castomResource.read_only" },
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { IdentityServerScopeConstants.ApiScope_Level1 }
                },


                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        "openid",
                        "profile",
                        IdentityServerScopeConstants.ApiScope_Level2 }
                },

                new Client
                {
                    ClientId = "test.client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //Області, до яких клієнт має доступ
                    AllowedScopes = {
                        IdentityServerScopeConstants.ApiScope_Read,
                        IdentityServerScopeConstants.ApiScope_Write,
                        IdentityServerScopeConstants.ApiScope_Delete}
                }
            };


    }
}