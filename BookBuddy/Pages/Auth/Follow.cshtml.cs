using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;

namespace BookBuddy.Pages.Auth;

public class FollowModel : PageModel
{
    private readonly DataStore _dataStore;

    public FollowModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<Models.Uporabnik> VsiUporabniki { get; set; } = new List<Models.Uporabnik>();

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

        current.Sledi ??= new List<Models.Uporabnik>();

        if (!current.Sledi.Contains(target))
        {
            current.Sledi.Add(target);
            _dataStore.Aktivnosti.Add($"{current.UporabniskoIme} sedaj sledi {target.UporabniskoIme}.");
        }
        else
        {
            current.Sledi.Remove(target);
            _dataStore.Aktivnosti.Add($"{current.UporabniskoIme} neha slediti {target.UporabniskoIme}.");
        }

        return RedirectToPage();
    }
}