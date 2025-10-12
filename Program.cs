using GhadsBot;
//HVORDAN DU BRUGER BOTTEN:
//1. Opret din egen bot på Telegram ved at sende besked til BotFather (https://t.me/botfather) og følg instruktionerne for at få en API-token.
//1.1 ELLER Brug min bot ved at søge efter @nickmoghadam_bot i Telegram og starte en chat med den ved at sende en besked.(SKIP SKRIDT 2)

//2. Kopier API-token og indsæt den i _token variablen i TelegramBot klassen
//3. Find dit chat ID ved at sende en besked til botten og derefter kalde GetUpdatesAsync metoden i koden nedenfor. Chat ID'et vil retuneres som id.
//4. Indsæt dit chat ID i _chatId variablen i TelegramBot klassen
//5. Kør programmet for at sende en besked til botten.

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
        bool running = true;

        PrintMenu();
        while (running)
        {
            string? input = Console.ReadLine();
            if (!Int32.TryParse(input, out int choice))
            {
                Console.WriteLine("Ugyldigt input, prøv igen");
            }
            else
            {
                
                switch (choice)
                {
                    case 0:
                        running = false;
                        break;
                    case 1:
                        Console.WriteLine("Indtast besked til at sende til botten:");
                        string? message = Console.ReadLine();
                        try
                        {
                            await bot.SendMessageAsync(message);
                        }catch(NullReferenceException nre){
                            Console.WriteLine("Beskeden må ikke være tom, prøv igen" + nre.Message);
                        }finally{
                            PrintMenu();
                        }
                        
                        break;
                    case 2:
                        await bot.GetUpdatesAsync(); //henter beskeder sendt til botten igennem getUpdates http request. ID'et den retunerer vil være dit chatID
                        break;
                    default:
                        Console.WriteLine("Ugyldigt input, prøv igen");
                        break;
                }

            }
        }

    }
    public static void PrintMenu()
    {
        Console.WriteLine("Vælg en mulighed:");
        Console.WriteLine("1. Send besked til botten");
        Console.WriteLine("2. Hent seneste besked sendt til botten"); 
        Console.WriteLine("0. Afslut");
    }
}
