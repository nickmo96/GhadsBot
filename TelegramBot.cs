using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

//bruger Newtonsoft.Json til at parse json data
namespace GhadsBot;

public class TelegramBot
{
    private readonly string _token = "8393432003:AAFCByo9c06ZvAd2U1SHvDo-bq-h26sp-M8";
    private readonly string _chatId = "7091701318"; //chatId til min test chat med botten, skal ændres til at være dynamisk senere
    private readonly HttpClient _client;

    public TelegramBot()
    {
        _client = new HttpClient();
    }

    public async Task SendMessageAsync(string message) //kan også tilføje chatId som parameter, for at sende til forskellige chats
    {
        string url = $"https://api.telegram.org/bot{_token}/sendMessage?chat_id={_chatId}&text={message}";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        var response = await _client.SendAsync(request);
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        //Console.WriteLine(response.StatusCode); kan bruges til debugging
    }

    public async Task GetUpdatesAsync()
    {
        string url = $"https://api.telegram.org/bot{_token}/getUpdates";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _client.SendAsync(request);
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        //Console.WriteLine(response.StatusCode); 
    }

    public async Task ShowAllUpdatesAsync()
    {
        string request = $"https://api.telegram.org/bot{_token}/getUpdates";
        string response = await _client.GetStringAsync(request);
        Console.WriteLine("Alle updates fra TelegramBot:");
        Console.WriteLine(response);
    }



    public async Task ListenAsync() //TODO ikke færdig implementeret
    {
        long offset = 0; //holder styr på hvilken besked der er læst så vi ikke læser den samme besked flere gange

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.telegram.org/bot{_token}/getUpdates?offset={offset}");
        var response = await _client.SendAsync(request);
        //response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);


        JObject json = JObject.Parse(content); //parser json data fra response
        JToken? messages = json["result"]; //henter som json array der indeholder alle parsede json objekter 

        foreach(JToken item in messages)
        {
            if(item != null)
            {
                long updateId = (long)item["update_id"];
                string messageText = item["message"]?["text"]?.ToString();
                string chatId = item["message"]?["chat"]?["id"]?.ToString();
                string entity = item["message"]?["entities"]?[0]?["type"]?.ToString();
            }
        }
        

        // if (messages != null)


        //       offset = (long)update["update_id"] + 1;
        //     }
    }
}

            //await Task.Delay(1000);
        
    


