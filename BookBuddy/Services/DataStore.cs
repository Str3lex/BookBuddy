using BookBuddy.Models;

namespace BookBuddy.Services;

public static class DataStore
{
    public static List<Uporabnik> Uporabniki = new();
    public static List<Knjiga> Knjige = new();

    public static Uporabnik? TrenutniUporabnik { get; set; }
    public static List<Mnenje> Mnenja = new();
}
