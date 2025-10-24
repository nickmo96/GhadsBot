using GhadsBot.Database;
using GhadsBot.Service;
using System.Net.Http.Headers;
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

//TODO: MVC, GUI, give ander adgang til botten  gennem besked eller website, sende beskeder til andre brugere, lave commands til botten 
//TODO: Lave HTML parser og indehendte data fra en hjemmeside og sende det som besked, Tilføj User og Logging
//TODO: TUI OG COmmands til HAshMap og ENUM

//TODO: MVC, GUI, sende beskeder til andre brugere
//TODO: Tilføj Logging. 

//TODO: LAV TUI OG COMMANDS TIL HASHMAPS, MÅSKE LAVER ENUMS TIL COMMANDS

{
    private async static Task Main(string[] args)
    {
        TelegramBot bot = new TelegramBot();
        //bot.ListenAsync();
        //bot.CommandListenerAsync();
        Task.Run (() => bot.CommandListenerAsync()); 
        bool running = true;

        PrintMenu();
        while (running)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (!Int32.TryParse(input, out int choice))
            {
                Console.Write("Ugyldigt input, prøv igen");
            }
            else
            {
                
                
                switch (choice)
                {
                    case 0:
                        running = false;
                        break;
                    case 1:
                        Console.WriteLine("Indtast besked til at sende til botten:"); //case 1 skal finjusteret
                        string? message = Console.ReadLine();
                        long? id = await bot.GetChatIdAsync();
                        if (id != null && message != null)
                        {
                            await bot.SendMessageAsync(message, id.Value.ToString());
                        }
                        {
                            Console.WriteLine("Ingen chatId fundet. Send en besked til botten først.\n ønsker du at sende en besked til min bot? (ja/nej)");
                            string? response = Console.ReadLine();
                            if (response != null && message != null && response.ToLower() == "ja")
                            {
                                long botChatId = 7091701318; // Mit chatId til test med botten
                                await bot.SendMessageAsync(message, botChatId.ToString());
                                break;

                        }
                            else
                            {
                                Console.WriteLine("Besked ikke sendt. Afslutter.");
                                break;
                            }
                        }
                    case 2:
                        await bot.GetUpdatesAsync(); //henter beskeder sendt til botten igennem getUpdates http request. ID'et den retunerer vil være dit chatID
                        break;
                    case 3:
                          long chatId = (long)await bot.GetChatIdAsync(); //viser alle beskeder sendt til botten
                        break;
                    case 4:
                        Console.WriteLine("Skriv besked");
                        string? msgToAll = Console.ReadLine();
                        if(msgToAll != null)
                        {
                            await bot.SendMessageToAllAsync(msgToAll);
                        }
                       
                        break;
                    case 5:
                        Console.WriteLine("Skriv besked til Benyamin");
                        string? msg = Console.ReadLine();
                        if(msg == null)
                        {
                            Console.WriteLine("Besked er tom, prøv igen");
                            break;
                        }
                        await bot.SendMessageAsync(msg, "7988478482"); //min chatId
                        break;
                    case 6:
                        DBPerson db = new DBPerson();
                        Console.WriteLine("alle personer i DB");
                       foreach(Person person in db.GetAllPersons())
                        {
                            Console.WriteLine(person.ToString());
                        }
                        break;
                    case 7:
                        Console.WriteLine("skriv chatID");
                        string? chatID = Console.ReadLine();
                        long.TryParse(chatID, out long parsedChatID);

                        Console.WriteLine("skriv fornavn");
                        string? firstName = Console.ReadLine();

                        Console.WriteLine("skriv efternavn");
                        string? lastName = Console.ReadLine();

                        Console.WriteLine("skriv brugernavn");
                        string? username = Console.ReadLine();

                        Person p = new Person(parsedChatID, firstName, lastName, username);
                        DBPerson dbPerson1 = new DBPerson();
                        dbPerson1.InsertPerson(p);
                        Console.WriteLine(p.ToString());
                        break;
                    case 8:
                        Console.WriteLine("skriv chatId for at finde person");
                        string? cID = Console.ReadLine();
                        long.TryParse(cID, out long parsedID);
                        DBPerson dbp = new DBPerson();
                        Person? personById = await dbp.GetPersonByChatIDAsync(parsedID);

                        if (personById != null)
                            Console.WriteLine(personById.ToString());
                        else
                            Console.WriteLine("Ingen person fundet med det chatId.");
                        break;

                    case 9:
                        Console.WriteLine("skriv chatId for at slette person");
                        string? chatIdToDelete = Console.ReadLine();
                        long.TryParse(chatIdToDelete, out long parsedChatIdToDelete);
                        DBPerson dbPerson = new DBPerson();
                        dbPerson.DeletePersonByChatId(parsedChatIdToDelete);
                        break;
                    case 10:
                        HTMLParser parser = new HTMLParser();
                        await parser.GetTemperatureCPH();
                        break;
                    case 11:
                        HTMLParser  parser1 = new HTMLParser();
                        
                        await parser1.GetIranianNews();
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
        Console.WriteLine("2. Hent seneste besked sendt til botten - VIRKER IKKE NÅR LISTENER KØRE");
        Console.WriteLine("3. Hent chat ID");
        Console.WriteLine("4. Send besked til alle");
        Console.WriteLine("5. Send besked til Benyamin");
        Console.WriteLine("6. hent alle person fra DB");
        Console.WriteLine("7. Opret person objekt");
        Console.WriteLine("8. Hent person fra DB ved chatID");
        Console.WriteLine("9. Slet person fra DB ved chatID");
        Console.WriteLine("0. Afslut");
        
    }
}
