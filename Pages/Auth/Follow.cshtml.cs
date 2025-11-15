using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using User = BookBuddy.Models.Uporabnik;  // Uporabite alias

namespace BookBuddy.Pages.Auth;

public class FollowModel : PageModel
{
    private readonly DataStore _dataStore;

    public FollowModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<User> VsiUporabniki { get; set; } = new List<User>();
    public User? TrenutniUporabnik => _dataStore.TrenutniUporabnik;  // Dodajte to lastnost

    public void OnGet()
    {
        VsiUporabniki = _dataStore.Uporabniki
            .Where(u => u != _dataStore.TrenutniUporabnik)
            .ToList();
    }

    public IActionResult OnPost(string followUser)
    {
        var current = _dataStore.TrenutniUporabnik;
        var target = _dataStore.Uporabniki.FirstOrDefault(u => u.UporabniskoIme == followUser);

        if (current == null || target == null)
            return Page();

        // Inicializiraj Sledi seznam, če še ne obstaja
        if (current.Sledi == null)
            current.Sledi = new List<User>();

        if (!current.Sledi.Any(s => s.UporabniskoIme == target.UporabniskoIme))
        {
            current.Sledi.Add(target);
            _dataStore.Aktivnosti.Add($"{current.UporabniskoIme} sedaj sledi {target.UporabniskoIme}.");
        }
        else
        {
            current.Sledi.RemoveAll(s => s.UporabniskoIme == target.UporabniskoIme);
            _dataStore.Aktivnosti.Add($"{current.UporabniskoIme} neha slediti {target.UporabniskoIme}.");
        }

        return RedirectToPage();
    }

    // Pomožna metoda za preverjanje ali trenutni uporabnik že sledi
    public bool AliSlediUporabniku(string uporabniskoIme)
    {
        var current = _dataStore.TrenutniUporabnik;
        return current?.Sledi?.Any(s => s.UporabniskoIme == uporabniskoIme) == true;
    }
}