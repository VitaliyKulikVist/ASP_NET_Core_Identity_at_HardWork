using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Start:
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nВиберiть варiант:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"1: GrantTypes.ClientCredentials");
            Console.WriteLine($"2: GrantTypes.Code");
            Console.ResetColor();

            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    _ = SendRequestAtIdentityServerAtTryGetAccesFromApi1Async("client", "secret");
                    break;
                case 2:
                    _ = SendRequestAtIdentityServerAtTryGetAccesFromApi1Async("redirectClient", "secret");
                    break;
                default:
                    break;
            }

            int input2 = Convert.ToInt32(Console.ReadLine());
            if(input2 == 0)
            {
                goto Start;
            }
        }
        private static async Task SendRequestAtIdentityServerAtTryGetAccesFromApi1Async(string clientID, string clientSecret)
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
                ClientId = clientID,
                ClientSecret = clientSecret,
            });

            if (tokenResponse.IsError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error =" + tokenResponse.Error);
                Console.ResetColor();

                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            var parserJson = JsonConvert.DeserializeObject(tokenResponse.Json.ToString());
            Console.WriteLine(parserJson);
            Console.WriteLine("\n\n");
            Console.ResetColor();

            // call api
            Console.WriteLine("\t\tIdentity");
            await Switches1(tokenResponse);

            Console.WriteLine("\t\tWeatherForecast");
            await Switches2(tokenResponse);

            Console.ResetColor();
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
