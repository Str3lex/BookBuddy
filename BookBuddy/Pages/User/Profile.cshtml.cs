using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.User
{
    public class ProfileModel : PageModel
    {
        [BindProperty] public string Ime { get; set; } = "";
        [BindProperty] public string Priimek { get; set; } = "";
        [BindProperty] public string NajljubsaZvrst { get; set; } = "";

        public string? Sporocilo { get; set; }

        public void OnGet()
        {
            var u = DataStore.TrenutniUporabnik;

            if (u != null)
            {
                Ime = u.Ime;
                Priimek = u.Priimek;
                NajljubsaZvrst = u.NajljubsaZvrst;
            }
        }

        public IActionResult OnPost()
        {
            var u = DataStore.TrenutniUporabnik;
            if (u == null)
                return RedirectToPage("/Auth/Login");

            u.Ime = Ime;
            u.Priimek = Priimek;
            u.NajljubsaZvrst = NajljubsaZvrst;

            Sporocilo = "Profil uspešno posodobljen!";
            return Page();
        }
    }
}