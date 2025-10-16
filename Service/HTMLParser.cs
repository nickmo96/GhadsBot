using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhadsBot.Service
{
    public class HTMLParser
    {
        private readonly string url;
        public HTMLParser()
        {
        }


        public async Task GetTempatureCPH()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://api.open-meteo.com/v1/metno?latitude=55.6761&longitude=12.5683&current=temperature_2m"
            );
            var response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();

            if(res.Contains("temperature_2m"))
            {
                int startIndex = res.IndexOf("temperature_2m") + "temperature_2m".Length + 3; // +3 for '": '
                int endIndex = res.IndexOf(",", startIndex);
                string temperature = res.Substring(startIndex, endIndex - startIndex);
                Console.WriteLine($"Temperaturen i KBH er: {temperature}°C");
            }
        }
    }
}
