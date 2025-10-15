public class Person
{
    public long ChatId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }


    public Person(long chatId, string? firstName, string? lastName, string? username)
    {
        ChatId = chatId;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
    }
    public override string ToString()
    {
        return $"ChatId: {ChatId}, FirstName: {FirstName}, LastName: {LastName}, Username: {Username}";
    }
}
