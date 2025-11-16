namespace BookBuddy.Models;

public class Uporabnik
{
    public string UporabniskoIme { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Geslo { get; set; } = string.Empty;
    public string Ime { get; set; } = string.Empty;
    public string Priimek { get; set; } = string.Empty;
    public string NajljubsaZvrst { get; set; } = string.Empty;
    public List<Uporabnik> Sledi { get; set; } = new List<Uporabnik>();
}
