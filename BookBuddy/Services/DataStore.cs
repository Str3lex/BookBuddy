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
    public string CurrentTheme { get; set; } = "light";

    // METODE ZA RAZVRŠČANJE
    public List<Knjiga> RazvrstiKnjige(string sortBy, string sortOrder)
    {
        var knjige = Knjige.AsEnumerable();

        switch (sortBy.ToLower())
        {
            case "naslov":
                knjige = sortOrder.ToLower() == "desc" 
                    ? knjige.OrderByDescending(k => k.Naslov)
                    : knjige.OrderBy(k => k.Naslov);
                break;
            case "avtor":
                knjige = sortOrder.ToLower() == "desc" 
                    ? knjige.OrderByDescending(k => k.Avtor)
                    : knjige.OrderBy(k => k.Avtor);
                break;
            case "zanr":
                knjige = sortOrder.ToLower() == "desc" 
                    ? knjige.OrderByDescending(k => k.Zanr)
                    : knjige.OrderBy(k => k.Zanr);
                break;
            case "leto":
                knjige = sortOrder.ToLower() == "desc" 
                    ? knjige.OrderByDescending(k => k.LetoIzdaje)
                    : knjige.OrderBy(k => k.LetoIzdaje);
                break;
            case "rating":
                knjige = sortOrder.ToLower() == "desc" 
                    ? knjige.OrderByDescending(k => k.Rate)
                    : knjige.OrderBy(k => k.Rate);
                break;
            case "status":
                knjige = sortOrder.ToLower() == "desc" 
                    ? knjige.OrderByDescending(k => k.Status)
                    : knjige.OrderBy(k => k.Status);
                break;
            default:
                knjige = knjige.OrderBy(k => k.Naslov);
                break;
        }

        return knjige.ToList();
    }

    public List<string> VsiZanri()
    {
        return Knjige.Select(k => k.Zanr).Distinct().OrderBy(z => z).ToList();
    }

    // METODE ZA UREJANJE
    public void PosodobiKnjigo(Knjiga posodobljenaKnjiga)
    {
        var obstojecaKnjiga = Knjige.FirstOrDefault(k => k.Id == posodobljenaKnjiga.Id);
        if (obstojecaKnjiga != null)
        {
            obstojecaKnjiga.Naslov = posodobljenaKnjiga.Naslov;
            obstojecaKnjiga.Avtor = posodobljenaKnjiga.Avtor;
            obstojecaKnjiga.Zanr = posodobljenaKnjiga.Zanr;
            obstojecaKnjiga.LetoIzdaje = posodobljenaKnjiga.LetoIzdaje;
            obstojecaKnjiga.Status = posodobljenaKnjiga.Status;
            obstojecaKnjiga.Rate = posodobljenaKnjiga.Rate;
        }
    }

    public void IzbrisiKnjigo(int knjigaId)
    {
        var knjiga = Knjige.FirstOrDefault(k => k.Id == knjigaId);
        if (knjiga != null)
        {
            Knjige.Remove(knjiga);
        }
    }
}
