using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Books;

public class RandomModel : PageModel
{
    private readonly DataStore _dataStore;
    private static readonly Random _random = new Random();

    public RandomModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public string RandomBook { get; set; } = string.Empty;

    public void OnGet()
    {
        IzberiNakljucnoKnjigo();
    }

    public IActionResult OnPost()
    {
        IzberiNakljucnoKnjigo();
        return RedirectToPage();
    }

    private void IzberiNakljucnoKnjigo()
    {
        if (_dataStore.Knjige != null && _dataStore.Knjige.Any())
        {
            var nakljucniIndex = _random.Next(_dataStore.Knjige.Count);
            var izbranaKnjiga = _dataStore.Knjige[nakljucniIndex];
            RandomBook = $"{izbranaKnjiga.Naslov} - {izbranaKnjiga.Avtor} ({izbranaKnjiga.Zanr})";

            _dataStore.Aktivnosti.Add($"{_dataStore.TrenutniUporabnik?.UporabniskoIme ?? "Gost"} je pregledal naključno knjigo: {izbranaKnjiga.Naslov}");
        }
        else
        {
            RandomBook = "Ni knjig na voljo";
        }
    }
}