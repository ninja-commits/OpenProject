using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientCalculatorService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AuthConfig config = CreateAuth.ReadJsonFromFile("appsettings.json");

            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            var scopes = new string[] { config.ResourceId };

            try
            {
                var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                Console.WriteLine(result.AccessToken);

                if (!string.IsNullOrEmpty(result.AccessToken))
                {
                    var httpClient = new HttpClient();
                    var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

                    if (defaultRequestHeaders.Accept == null ||
                       !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new
                          MediaTypeWithQualityHeaderValue("application/json"));
                    }

                    defaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("bearer", result.AccessToken);

                    HttpResponseMessage response = await httpClient.GetAsync("https://localhost:44360/api/values/Get");
                    if (response.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        string json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(json);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed to call the Web Api: {response.StatusCode}");
                        string content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Content: {content}");
                    }
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            Console.WriteLine(config.ClientSecret);
            Console.WriteLine("Hello World!");
        }
    }
}
