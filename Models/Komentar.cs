namespace BookBuddy.Models;

public class Komentar
{
    public string Uporabnik { get; set; } = string.Empty;
    public string Besedilo { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
}