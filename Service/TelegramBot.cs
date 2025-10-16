using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

//bruger Newtonsoft.Json til at parse json data
namespace GhadsBot.Service;

public class TelegramBot
{
    private readonly string _token = "8393432003:AAFCByo9c06ZvAd2U1SHvDo-bq-h26sp-M8";
    //private readonly string _mitChatId = "7091701318"; //chatId til min test chat med botten, skal ændres til at være dynamisk senere
    private readonly HttpClient _client;
    private long _offset; //holder styr på hvilken besked der er læst så vi ikke læser den samme besked flere gange

    public TelegramBot()
    {
        _client = new HttpClient();
        _offset = 0; 
    }

    public async Task SendMessageAsync(string message, string chatId)  
    {
        string url = $"https://api.telegram.org/bot{_token}/sendMessage?chat_id={chatId}&text={message}";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        var response = await _client.SendAsync(request);
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        //Console.WriteLine(response.StatusCode); kan bruges til debugging
    }
    public async Task<long?>  GetChatIdAsync() //henter chatId'et til den bruger der sender beskeden til botten
    {
        string url = $"https://api.telegram.org/bot{_token}/getUpdates";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _client.SendAsync(request);
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);

        JObject json = JObject.Parse(content); //parser json data fra response
        JToken? messages = json["result"]; //henter som json array der indeholder alle parsede json objekter 
        long? res = null;
        if (messages != null && messages.HasValues)
        {
             foreach (JToken item in messages)
        {
        if (item != null)
        {
            string? chatId = item["message"]?["chat"]?["id"]?.ToString();
            if (chatId != null)
            {
                long parsedId = long.Parse(chatId);
                Console.WriteLine($"Chat ID: {parsedId}");
                res = parsedId;
                
            }
        }
    }
        }
       

   

        return res; //returnerer null hvis der ikke er nogen beskeder   
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
    


    public async Task CommandListenerAsync() //TODO VIRKER MEN IKKE OPTIMALT LÆSER BESKEDER NÅR MAN SKAL BRUGE DEM SENERE FIXIFIIXFIFXI
    //overvej at dele metoden op i mindre metoder
    {
        bool listening = true;
        while (listening)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.telegram.org/bot{_token}/getUpdates?offset={_offset}");
            var response = await _client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(content);


            JObject json = JObject.Parse(content); //parser json data fra response
            JToken? messages = json["result"]; //henter som json array der indeholder alle parsede json objekter 

            if (messages != null)
                foreach (JToken item in messages)
                {
                    if (item != null)
                    {

                        long? updateId = (long)item["update_id"];
                        string? messageText = item["message"]?["text"]?.ToString();
                        string? chatId = item["message"]?["chat"]?["id"]?.ToString();
                        string? entity = item["message"]?["entities"]?[0]?["type"]?.ToString(); //tjekker om beskeden er en kommando
                        if (entity == "bot_command")
                        {
                            switch (messageText)
                            {
                                case "/hej":
                                    await SendMessageAsync("HEJSA", chatId!);
                                    break;
                                case "/help":
                                    await SendMessageAsync("kommandoer: /hej - Bot siger hej\n /nadia \n /help - Vis denne besked", chatId);
                                    break;
                                case "/nadia":
                                    for (int i = 0; i < 10; i++)
                                    {
                                        await SendMessageAsync("Nadia er smuk", chatId);
                                    }
                                    break;
                                default:
                                    await SendMessageAsync("Ugyldig kommando. prøv /help", chatId);
                                    break;
                            }
                            Console.WriteLine($"Ny kommando modtaget: {messageText} fra chat ID: {chatId}");
                        }
                        else
                        {
                            Console.WriteLine($"Ny besked modtaget: {messageText} fra chat ID: {chatId}");
                        }
                        _offset = (long)(updateId + 1); //opdaterer offset til næste besked
                    }

                }
            await Task.Delay(1000);

        }

    }
  

}

          
        
    


