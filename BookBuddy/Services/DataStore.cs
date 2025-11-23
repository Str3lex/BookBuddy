using BookBuddy.Models;
using BookBuddy.Data;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.Services;

public class DataStore
{
    private readonly AppDbContext _context;

    public DataStore(AppDbContext context)
    {
        _context = context;
    }

    private void LoadInitialData()
    {
        // Load data from database
        if (!Knjige.Any() && _context.Knjige.Any())
        {
            Knjige.AddRange(_context.Knjige.ToList());
        }
        if (!Uporabniki.Any() && _context.Uporabniki.Any())
        {
            Uporabniki.AddRange(_context.Uporabniki.ToList());
        }
        if (!Komentarji.Any() && _context.Komentarji.Any())
        {
            Komentarji.AddRange(_context.Komentarji.ToList());
        }
        if (!Mnenja.Any() && _context.Mnenja.Any())
        {
            Mnenja.AddRange(_context.Mnenja.ToList());
        }
    }

    public List<Uporabnik> Uporabniki { get; set; } = new List<Uporabnik>();
    public List<Knjiga> Knjige { get; set; } = new List<Knjiga>();
    public List<string> Aktivnosti { get; set; } = new List<string>();
    public List<Knjiga> IzbraneKnjige { get; set; } = new List<Knjiga>();
    public List<Komentar> Komentarji { get; set; } = new List<Komentar>();
    public Uporabnik? TrenutniUporabnik { get; set; }
    public List<Mnenje> Mnenja { get; set; } = new();
    public string CurrentTheme { get; set; } = "light";

    // Save methods for database persistence
    public void SaveKnjiga(Knjiga knjiga)
    {
        _context.Knjige.Add(knjiga);
        _context.SaveChanges();
        Knjige.Add(knjiga);
    }

    public void DeleteKnjiga(int knjigaId)
    {
        var knjiga = _context.Knjige.FirstOrDefault(k => k.Id == knjigaId);
        if (knjiga != null)
        {
            _context.Knjige.Remove(knjiga);
            _context.SaveChanges();
            Knjige.RemoveAll(k => k.Id == knjigaId);
        }
    }

    public void SaveKomentar(Komentar komentar)
    {
        _context.Komentarji.Add(komentar);
        _context.SaveChanges();
        Komentarji.Add(komentar);
    }

    public void SaveUporabnik(Uporabnik uporabnik)
    {
        _context.Uporabniki.Add(uporabnik);
        _context.SaveChanges();
        Uporabniki.Add(uporabnik);
    }

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

            // Update in database
            var dbKnjiga = _context.Knjige.FirstOrDefault(k => k.Id == posodobljenaKnjiga.Id);
            if (dbKnjiga != null)
            {
                dbKnjiga.Naslov = posodobljenaKnjiga.Naslov;
                dbKnjiga.Avtor = posodobljenaKnjiga.Avtor;
                dbKnjiga.Zanr = posodobljenaKnjiga.Zanr;
                dbKnjiga.LetoIzdaje = posodobljenaKnjiga.LetoIzdaje;
                dbKnjiga.Status = posodobljenaKnjiga.Status;
                dbKnjiga.Rate = posodobljenaKnjiga.Rate;
                _context.SaveChanges();
            }
        }
    }
    public void IzbrisiKnjigo(int knjigaId)
    {
        // Najprej najdi iz IN-MEMORY liste
        var knjiga = Knjige.FirstOrDefault(k => k.Id == knjigaId);

        // Odstrani iz liste
        Knjige.RemoveAll(k => k.Id == knjigaId);

        // Poskusi odstranit tudi iz DB, če obstaja
        var knjigaDb = _context?.Knjige.FirstOrDefault(k => k.Id == knjigaId);
        if (knjigaDb != null)
        {
            _context.Knjige.Remove(knjigaDb);
            _context.SaveChanges();
        }
    }


    public void RegistrirajUporabnika(Uporabnik novUporabnik)
    {
        // Dodeli ID če ga še nima
        if (novUporabnik.Id == 0)
        {
            novUporabnik.Id = Uporabniki.Any() ? Uporabniki.Max(u => u.Id) + 1 : 1;
        }

        Uporabniki.Add(novUporabnik);
        _context.Uporabniki.Add(novUporabnik);
        _context.SaveChanges();

        TrenutniUporabnik = novUporabnik;
    }

    // Lahko dodaš tudi metodo za odjavo
    public void OdjaviUporabnika()
    {
        TrenutniUporabnik = null;
    }
}