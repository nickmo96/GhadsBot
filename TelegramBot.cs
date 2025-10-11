using System.Net.Http;
using System.Threading.Tasks;

namespace GhadsBot;

public class TelegramBot
{
    private readonly string _token = "8393432003:AAFCByo9c06ZvAd2U1SHvDo-bq-h26sp-M8";
    private readonly string _chatId = "7091701318";
    private readonly HttpClient _client;

    public TelegramBot()
    {
        _client = new HttpClient();
    }

    public async Task SendMessageAsync(string message) //kan også tilføje chatId som parameter
    {
        string url = $"https://api.telegram.org/bot{_token}/sendMessage?chat_id={_chatId}&text={message}";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        var response = await _client.SendAsync(request); 
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        //Console.WriteLine(response.StatusCode); kan bruges til debugging
    }
}
