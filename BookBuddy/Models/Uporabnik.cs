namespace BookBuddy.Models;

public class Uporabnik
{
    public int Id { get; set; }
    public string UporabniskoIme { get; set; } = "";
    public string Eposta { get; set; } = "";
    public string Geslo { get; set; } = "";
    public string Ime { get; set; } = "";
    public string Priimek { get; set; } = "";
    public string NajljubsaZvrst { get; set; } = "";
}
