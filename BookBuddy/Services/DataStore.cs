using BookBuddy.Models;

namespace BookBuddy.Services;

public class DataStore
{
    public List<Uporabnik> Uporabniki { get; set; } = new List<Uporabnik>();
    public List<Knjiga> Knjige { get; set; } = new List<Knjiga>();
    public List<string> Aktivnosti { get; set; } = new List<string>();
    public List<Knjiga> IzbraneKnjige { get; set; } = new List<Knjiga>();
    public List<Komentar> Komentarji { get; set; } = new List<Komentar>();
    public Uporabnik? TrenutniUporabnik { get; set; }
    public static List<Mnenje> Mnenja = new();

}
