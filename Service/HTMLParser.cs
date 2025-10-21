using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GhadsBot.Service
{
    public class HTMLParser
    {
        private readonly string url;
        public HTMLParser()
        {
        }


        public async Task<double> GetTemperatureCPH()
        {
            double result = 0;
            var client = new HttpClient();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.open-meteo.com/v1/metno?latitude=55.6761&longitude=12.5683&current=temperature_2m"
            );

            var response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(res);
            string? tempString = json["current"]?["temperature_2m"]?.ToString();

            if (double.TryParse(tempString, out double temperatur))
            {
                Console.WriteLine($"Temperaturen i København er: {temperatur}°C");
                result = temperatur;
            }
            else
            {
                Console.WriteLine("Kunne ikke hente temperaturen fra JSON-dataen.");
            }

            return result;
        }

        public async Task<string> GetIranianNews()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.worldnewsapi.com/search-news?api-key=0e425745e9b44202b13a23c840e5337b&source-country=ir&language=en"
            );
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
