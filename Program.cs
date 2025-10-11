internal class Program
{
    private async static Task Main(string[] args)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.telegram.org/bot8393432003:AAFCByo9c06ZvAd2U1SHvDo-bq-h26sp-M8/sendMessage?chat_id=7091701318&text=din fars"
        );

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(content);
    }
}
