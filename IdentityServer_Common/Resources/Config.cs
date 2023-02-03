using IdentityModel;
using IdentityServer_Common.Constants;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer_Common.Resources
{
    public static class Config
    {
        /// <summary>
        /// Ресурси до яких буде матиме доступ клієнт
        /// </summary>
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

        /// <summary>
        /// Область: те, що можна використовувати клієнському додатку
        /// </summary>
        /// <remarks>
        /// Доступ на рівні обласі(які представлені у вигляді ресурсів які можуть бути IdentityResource або ApiResource)
        /// </remarks>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(IdentityServerScopeConstants.ApiScope_Level1, $"My {IdentityServerScopeConstants.ApiScope_Level1}"),
                new ApiScope(IdentityServerScopeConstants.ApiScope_Level2, $"My {IdentityServerScopeConstants.ApiScope_Level2}"),

                new ApiScope(name: IdentityServerScopeConstants.ApiScope_Read,   displayName: "Read your data."),
                new ApiScope(name: IdentityServerScopeConstants.ApiScope_Write,  displayName: "Write your data."),
                new ApiScope(name: IdentityServerScopeConstants.ApiScope_Delete, displayName: "Delete your data.")
            };

        /// <summary>
        /// Це необхідно для додаткового захисту інформації, якщо їх використовувати то для кожної АРІ прийшлось би створювати свій ТОКЕН
        /// </summary>
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
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { IdentityServerScopeConstants.ApiScope_Level1 },
                    //They contein IP adressa API
                    AllowedCorsOrigins = { "http://localhost:7249", "https://localhost:6249" },
                },

                new Client
                {
                    ClientId = "redirectClient",
                    ClientName = "This client realization a redirect technology",

                    RequireClientSecret = true,//те чи будемо використовувати ClientSecrets
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RequirePkce = true,//потрібен ключ підтвердження для Авторізейшен Код
                    AllowedGrantTypes = GrantTypes.Code,

                    //Перенаправлення після автентифікації клієнтського застосунку
                    RedirectUris = { "https://localhost:44300/signin-oidc", "https://oauth.pstmn.io/v1/callback" },
                    //Набір адрес яким дозволено використовувати IdentityServer(іншими словами набір арі)
                    AllowedCorsOrigins = { "http://localhost:7249", "https://localhost:6249" },
                    //Адреса куди відбувається перенаправлення після виходу з клієнтського застосунку
                    //PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44300/Auth/Login" },

                    //FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    
                    AllowOfflineAccess = true,
                    //Області які доступні цьому клієнту
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,// "openid",
                        IdentityServerConstants.StandardScopes.Profile,//"profile",
                        IdentityServerScopeConstants.ApiScope_Level2 }
                }
            };
    }
}