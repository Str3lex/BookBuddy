using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly DataStore _dataStore;

    public RegisterModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [BindProperty]
    public string UporabniskoIme { get; set; } = string.Empty;

    [BindProperty]
    public string Eposta { get; set; } = string.Empty;

    [BindProperty]
    public string Geslo { get; set; } = string.Empty;

    public string? Sporocilo { get; set; }

    public IActionResult OnPost()
    {
        if (_dataStore.Uporabniki.Any(u => u.UporabniskoIme == UporabniskoIme))
        {
            Sporocilo = "Uporabnik že obstaja!";
            return Page();
        }

        var user = new Models.Uporabnik
        {
            UporabniskoIme = UporabniskoIme,
            Email = Eposta,
            Geslo = Geslo
        };

        _dataStore.Uporabniki.Add(user);
        _dataStore.TrenutniUporabnik = user;
        _dataStore.Aktivnosti.Add($"Nov uporabnik registriran: {UporabniskoIme}");

        return RedirectToPage("/Uporabnik/Profile");
    }
}