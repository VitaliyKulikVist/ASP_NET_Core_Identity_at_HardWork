using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            _ = SendRequestAtIdentityServerAtTryGetAccesFromApi1Async();

            Console.ReadKey();
        }
        private static async Task SendRequestAtIdentityServerAtTryGetAccesFromApi1Async()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(disco.Error);
                Console.ResetColor();

                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
            });

            if (tokenResponse.IsError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error =" + tokenResponse.Error);
                Console.ResetColor();

                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            Console.ResetColor();

            // call api
            Console.WriteLine("\t\tIdentity");
            await Switches1(tokenResponse);

            Console.WriteLine("\t\tWeatherForecast");
            await Switches2(tokenResponse);
        }

        private static async Task GetResponseFromURLAsync(string url, IdentityModel.Client.TokenResponse tokenResponse)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Status code=\t" + response.StatusCode);
                Console.ResetColor();
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(JArray.Parse(content));
                Console.ResetColor();
            }
        }

        private static Task Switches1(IdentityModel.Client.TokenResponse tokenResponse)
        {
            string url = "https://localhost:7249/identity";

            return GetResponseFromURLAsync(url, tokenResponse);
        }

        private static Task Switches2(IdentityModel.Client.TokenResponse tokenResponse)
        {
            string url = "https://localhost:7249/WeatherForecast";

            return GetResponseFromURLAsync(url, tokenResponse);
        }
    }
}
