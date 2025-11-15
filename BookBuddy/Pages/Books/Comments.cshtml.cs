using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Books;

public class CommentsModel : PageModel
{
    private readonly DataStore _dataStore;

    public CommentsModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<Komentar> Komentarji => _dataStore.Komentarji;

    [BindProperty] 
    public string VnosKomentarja { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        if (!string.IsNullOrWhiteSpace(VnosKomentarja))
        {
            _dataStore.Komentarji.Add(new Komentar
            {
                Uporabnik = _dataStore.TrenutniUporabnik?.UporabniskoIme ?? "Gost",
                Besedilo = VnosKomentarja,
                Datum = DateTime.Now
            });

            _dataStore.Aktivnosti.Add($"{_dataStore.TrenutniUporabnik?.UporabniskoIme ?? "Gost"} je komentiral knjigo.");
        }

        return RedirectToPage();
    }
}