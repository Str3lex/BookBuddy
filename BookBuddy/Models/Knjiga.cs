namespace BookBuddy.Models;

public class Knjiga
{
    public int Id { get; set; }
    public string Naslov { get; set; } = string.Empty;
    public string Avtor { get; set; } = string.Empty;
    public string Zanr { get; set; } = string.Empty;
    public int LetoIzdaje { get; set; }
    public string Status { get; set; } = "Ni prebrana"; // Dodajte to lastnost
}