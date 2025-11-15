using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    public List<Knjiga> VseKnjige { get; set; } = new List<Knjiga>();
    public Knjiga? IzbranaKnjiga { get; set; }

    public void OnGet(int? id)
    {
        VseKnjige = _dataStore.Knjige.ToList();

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
        VseKnjige = _dataStore.Knjige.ToList();

        if (Id == 0)
        {
            Sporocilo = "Izberite knjigo!";
            return Page();
        }

        var knjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
        if (knjiga == null)
        {
            Sporocilo = "Knjiga ni najdena!";
            return Page();
        }

        var stariStatus = knjiga.Status;

        knjiga.Status = Status;

        _dataStore.Aktivnosti.Add($"{_dataStore.TrenutniUporabnik?.UporabniskoIme ?? "Gost"} je spremenil status knjige '{knjiga.Naslov}' iz '{stariStatus}' na '{Status}'");

        Sporocilo = $"Status knjige '{knjiga.Naslov}' je bil uspešno posodobljen na: {Status}";

        IzbranaKnjiga = knjiga;
        return Page();
    }
}
