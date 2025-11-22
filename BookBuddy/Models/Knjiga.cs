using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models;

public class Mnenje
{
    public int Id { get; set; }
    public int KnjigaId { get; set; }
    public int UporabnikId { get; set; }
    public string Besedilo { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
}

public class Knjiga
{
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
}

