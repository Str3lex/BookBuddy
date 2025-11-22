using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBuddy.Models;

public class Mnenje
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Knjiga")]
    public int KnjigaId { get; set; }
    public Knjiga? Knjiga { get; set; }

    public int UporabnikId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Besedilo { get; set; } = string.Empty;

    public DateTime Datum { get; set; } = DateTime.Now;
}

public class Knjiga
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Žanr je obvezen.")]
    public string Zanr { get; set; } = string.Empty;

    [Required(ErrorMessage = "Naslov je obvezen.")]
    public string Naslov { get; set; } = string.Empty;

    [Required(ErrorMessage = "Avtor je obvezen.")]
    public string Avtor { get; set; } = string.Empty;

    public int LetoIzdaje { get; set; }
    public string Status { get; set; } = "Ni prebrana";
    public int Rate { get; set; }

    // Navigation properties
    public List<Mnenje> Mnenja { get; set; } = new();
}