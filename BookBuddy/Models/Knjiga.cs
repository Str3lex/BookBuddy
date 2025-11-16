public class Mnenje
{
    public int Id { get; set; }
    public int KnjigaId { get; set; }
    public int UporabnikId { get; set; }
    public string Besedilo { get; set; } = "";
    public DateTime Datum { get; set; } = DateTime.Now;
}

public class Knjiga
{
    public int Id { get; set; }
    public string Naslov { get; set; } = "";
    public string Avtor { get; set; } = "";
    public string Status { get; set; } = "Ni prebrana";
    public int Rate { get; set; } = 0;

    public int LetoIzdaje { get; set; }
    public string Zvrst { get; set; } = "";
    public string Opis { get; set; } = "";
}
