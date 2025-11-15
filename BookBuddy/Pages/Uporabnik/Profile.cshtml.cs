using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Uporabnik
{
    public class ProfileModel : PageModel
    {
        private readonly DataStore _dataStore;

        public ProfileModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty]
        public string Ime { get; set; } = string.Empty;

        [BindProperty]
        public string Priimek { get; set; } = string.Empty;

        [BindProperty]
        public string NajljubsaZvrst { get; set; } = string.Empty;

        public string? Sporocilo { get; set; }

        public void OnGet()
        {
            var u = _dataStore.TrenutniUporabnik;

            if (u != null)
            {
                Ime = u.Ime ?? string.Empty;
                Priimek = u.Priimek ?? string.Empty;
                NajljubsaZvrst = u.NajljubsaZvrst ?? string.Empty;
            }
        }

        public IActionResult OnPost()
        {
            var u = _dataStore.TrenutniUporabnik;
            if (u == null)
                return RedirectToPage("/Auth/Login");

            u.Ime = Ime;
            u.Priimek = Priimek;
            u.NajljubsaZvrst = NajljubsaZvrst;

            _dataStore.Aktivnosti.Add($"{u.UporabniskoIme} je posodobil svoj profil");

            Sporocilo = "Profil uspešno posodobljen!";
            return Page();
        }
    }
}
