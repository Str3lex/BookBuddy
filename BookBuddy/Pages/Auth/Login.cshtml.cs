using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;

namespace BookBuddy.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty] public string UporabniskoIme { get; set; } = "";
        [BindProperty] public string Geslo { get; set; } = "";

        public string? Sporocilo { get; set; }

        public IActionResult OnPost()
        {
            var user = DataStore.Uporabniki
                .FirstOrDefault(u => u.UporabniskoIme == UporabniskoIme && u.Geslo == Geslo);

            if (user == null)
            {
                Sporocilo = "Napačno uporabniško ime ali geslo.";
                return Page();
            }

            DataStore.TrenutniUporabnik = user;

            return RedirectToPage("/User/Profile");
        }
    }
}