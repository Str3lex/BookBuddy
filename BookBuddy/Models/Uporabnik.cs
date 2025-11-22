using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models;

public class Uporabnik
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UporabniskoIme { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Geslo { get; set; } = string.Empty;

    public string Ime { get; set; } = string.Empty;
    public string Priimek { get; set; } = string.Empty;
    public string NajljubsaZvrst { get; set; } = string.Empty;

    // Navigation properties
    public List<Uporabnik> Sledi { get; set; } = new List<Uporabnik>();
}