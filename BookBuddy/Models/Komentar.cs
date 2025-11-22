using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models;

public class Komentar
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Uporabnik { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Besedilo { get; set; } = string.Empty;

    public DateTime Datum { get; set; } = DateTime.Now;

    // Dodaj povezavo do knjige
    public int KnjigaId { get; set; }
}