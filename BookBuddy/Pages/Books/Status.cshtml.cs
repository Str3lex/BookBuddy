using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books;

public class StatusModel : PageModel
{
    private readonly DataStore _dataStore;

    public StatusModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public string Status { get; set; } = "Ni prebrana";

    public string? Sporocilo { get; set; }

    public List<SelectListItem> VseKnjige { get; set; } = new List<SelectListItem>();
    public Knjiga? IzbranaKnjiga { get; set; }

    public void OnGet(int? id)
    {
        PripraviSeznamKnjig();

        if (id.HasValue)
        {
            Id = id.Value;
            IzbranaKnjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (IzbranaKnjiga != null)
            {
                Status = IzbranaKnjiga.Status;
            }
        }
    }

    public IActionResult OnPost()
    {
        if (Id == 0)
        {
            Sporocilo = "Izberite knjigo!";
            PripraviSeznamKnjig();
            return Page();
        }

        var knjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
        if (knjiga == null)
        {
            Sporocilo = "Knjiga ni najdena!";
            PripraviSeznamKnjig();
            return Page();
        }

        // Shrani stari status za aktivnost
        var stariStatus = knjiga.Status;

        // Posodobi status knjige
        knjiga.Status = Status;

        // Dodaj aktivnost
        _dataStore.Aktivnosti.Add($"{_dataStore.TrenutniUporabnik?.UporabniskoIme ?? "Gost"} je spremenil status knjige '{knjiga.Naslov}' iz '{stariStatus}' na '{Status}'");

        Sporocilo = $"Status knjige '{knjiga.Naslov}' je bil uspešno posodobljen na: {Status}";

        PripraviSeznamKnjig();
        IzbranaKnjiga = knjiga;
        return Page();
    }

    private void PripraviSeznamKnjig()
    {
        VseKnjige = _dataStore.Knjige
            .Select(k => new SelectListItem
            {
                Value = k.Id.ToString(),
                Text = $"{k.Naslov} - {k.Avtor}"
            })
            .ToList();
    }
}