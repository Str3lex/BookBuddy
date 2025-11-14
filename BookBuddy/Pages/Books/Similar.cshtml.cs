using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books;

public class SimilarModel : PageModel
{
    private readonly DataStore _dataStore;

    public SimilarModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<string> PodobneKnjige { get; set; } = new List<string>();

    public void OnGet()
    {
        var knjiga = _dataStore.IzbraneKnjige?.LastOrDefault();
        
        if (knjiga == null)
        {
            PodobneKnjige = new List<string>();
            return;
        }

        PodobneKnjige = _dataStore.Knjige
            .Where(k => k.Zanr == knjiga.Zanr && k.Naslov != knjiga.Naslov)
            .Select(k => $"{k.Naslov} - {k.Avtor} ({k.Zanr})")
            .ToList();
    }
}