using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GhadsBot.Database;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using GhadsBot.Model;
using GhadsBot.Service;

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
    
    public async Task SendMessageToAllAsync(string message)  
    {
        DBPerson dbp = new DBPerson();
        List<Person> persons = (List<Person>)dbp.GetAllPersons();
        foreach (Person p in persons)
        {
            await SendMessageAsync(message, p.ChatId.ToString());
        }
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


        JObject json = JObject.Parse(content); //parser json data fra response
        JToken? messages = json["result"]; //henter som json array der indeholder alle parsede json objekter
        if (messages != null && messages.HasValues)
        {
            foreach (JToken item in messages)
            {
                if (item != null)
                {
                    long chatId = (long)item["message"]?["chat"]?["id"];
                    string? firstName = item["message"]?["chat"]?["first_name"]?.ToString();
                    string? lastName = item["message"]?["chat"]?["last_name"]?.ToString();
                    string? username = item["message"]?["chat"]?["username"]?.ToString();

                   
                    DBPerson dbp = new DBPerson();

                    if (await dbp.GetPersonByChatIDAsync(chatId) == null)
                    {
                        
                        Person p = new Person(chatId, firstName, lastName, username);
                        Console.WriteLine($"Ny bruger fundet, tilføjer til database." + p.ToString());
                        dbp.InsertPerson(p);
                    }

                    string? messageText = item["message"]?["text"]?.ToString();
                    Console.WriteLine($"Chat ID: {chatId}, Message: {messageText}");
                }
            }
        }
        else
        {
            Console.WriteLine("Ingen nye beskeder.");
        }
        //Console.WriteLine(response.StatusCode); 
    }

    public async Task<string> FetchTelegramUpdatesUpdates()
    {
        string url = $"https://api.telegram.org/bot{_token}/getUpdates?offset={_offset}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _client.SendAsync(request);
        string content = await response.Content.ReadAsStringAsync();

        return content;

    }
     public async Task<List<JToken>> ParseTelegramUpdatesAsync()
    {
        string content = await FetchTelegramUpdatesUpdates();
        JObject json = JObject.Parse(content);
        List<JToken?> messages = json["result"].ToList();    
        return messages;  
    }

    public async Task SendNewsToUserAsync(string chatId)
    {
        string jsonString = await new HTMLParser().GetIranianNews();
        JObject json = JObject.Parse(jsonString);

        if (json["news"] is not JArray newsArray || newsArray.Count == 0)
        {
            await SendMessageAsync("Ingen iranske nyheder fundet.", chatId);
            return;
        }

        foreach (var item in newsArray)
        {
            long id = item["id"]?.ToObject<long>() ?? 0;
            string title = item["title"]?.ToString() ?? "";
            string summary = item["summary"]?.ToString() ?? "";
            string url = item["url"]?.ToString() ?? "";
            string pictureUrl = item["image"]?.ToString() ?? "";

            string message =
                $"📰 *{title}*\n\n" +
                $"{summary}\n\n" +
                $"🔗 [Læs mere]({url})";

            // Escape URL og tekst korrekt til Telegram API
            string encodedMessage = Uri.EscapeDataString(message);
            string sendUrl = $"https://api.telegram.org/bot{_token}/sendMessage?chat_id={chatId}&text={encodedMessage}&parse_mode=Markdown";

            await _client.GetAsync(sendUrl);
        }

        await SendMessageAsync($"Sendte {newsArray.Count} iranske nyheder.", chatId);
        Console.WriteLine($"Sendte {newsArray.Count} nyheder til chat {chatId}");
    }

    public async Task CommandListenerAsync()
    {
    while (true)
    {
        List<JToken>? messages = await ParseTelegramUpdatesAsync();

        if (messages != null && messages.Count > 0)
        {
            foreach (JToken item in messages)
            {
                JToken? message = item["message"];
                if (message == null) continue;

                long updateId = item.Value<long>("update_id");
                string? messageText = message["text"]?.ToString();
                string? entity = message["entities"]?[0]?["type"]?.ToString();
                string dynamicChatId = message["chat"]?["id"]?.ToString() ?? "0";
                    long staticChatId = (long)item["message"]?["chat"]?["id"];
                DBPerson dbp = new DBPerson();
                if(dbp.GetPersonByChatIDAsync(staticChatId).Result == null)
                {
                    string? firstName = item["message"]?["chat"]?["first_name"]?.ToString();
                    string? lastName = item["message"]?["chat"]?["last_name"]?.ToString();
                    string? username = item["message"]?["chat"]?["username"]?.ToString();

                    Person p = new Person(staticChatId, firstName, lastName, username);
                    Console.WriteLine($"Ny bruger fundet, tilføjer til database." + p.ToString());
                    dbp.InsertPerson(p);
                }

        
                if (entity == "bot_command")
                {
                        BotCommand command = ParseCommand(messageText);
                    switch (command)
                    {
                        case BotCommand.Hej:
                            await SendMessageAsync("Hej! Hvordan går det?", dynamicChatId);
                            break;

                        case BotCommand.Help:
                            await SendMessageAsync(
                                "Kommandoer:\n" +
                                "/hej - Bot siger hej\n" +
                                "/nadia - Spammer Nadia er smuk\n" +
                                "/temp - Viser temperatur i Kbh\n" +
                                "/news - Viser iranske nyheder\n" +
                                "/help - Viser denne besked", dynamicChatId
                            );
                            break;

                        case BotCommand.Nadia:
                            for (int i = 0; i < 10; i++)
                                await SendMessageAsync("Nadia er smuk", dynamicChatId);
                            break;

                        case BotCommand.Temp:
                            HTMLParser parser = new HTMLParser();
                            double temp = await parser.GetTemperatureCPH();
                            await SendMessageAsync($"Temperaturen i København er: {temp}°C", dynamicChatId);
                            break;
                        case BotCommand.News:
                                SendNewsToUserAsync(dynamicChatId).Wait();
                                break;

                            default:
                            await SendMessageAsync("Ukendt kommando. Prøv /help", dynamicChatId);
                            break;
                    }

                    Console.WriteLine($"Ny kommando modtaget: {messageText} fra chat ID: {dynamicChatId}");
                }
                else
                {
                    Console.WriteLine($"Ny besked modtaget: {messageText} fra chat ID: {dynamicChatId}");
                }

                _offset = updateId + 1;
            }
        }

        await Task.Delay(1000);
    }
}

    private BotCommand ParseCommand(string? input)
    {
        return input?.ToLower() switch
        {
            "/hej" => BotCommand.Hej,
            "/help" => BotCommand.Help,
            "/nadia" => BotCommand.Nadia,
            "/temp" => BotCommand.Temp,
            "/news" => BotCommand.News,
            _ => BotCommand.Unknown,
        };
    }
}
  



          
        
    


