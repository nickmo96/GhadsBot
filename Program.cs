using GhadsBot;


internal class Program
// BotFather fra Telegram
// https://core.telegram.org/bots/api  Telegram Bot API Documentation
// postman generet kode til http request og repsonen 
//TODO: MVC, UI, give ander adgang til botten  gennem besked eller website, sende beskeder til andre brugere, lave commands til botten 
//ide: indehendte data fra en hjemmeside og sende det som besked
{
    private async static Task Main(string[] args)
    {
        TelegramBot bot = new TelegramBot();
        await bot.SendMessageAsync("hallotest");
    }
}
