using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;

namespace BookBuddy.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly DataStore _dataStore;

        public LoginModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty]
        public string UporabniskoIme { get; set; } = string.Empty;

        [BindProperty]
        public string Geslo { get; set; } = string.Empty;

        public string? Sporocilo { get; set; }

        public IActionResult OnPost()
        {
            var user = _dataStore.Uporabniki
                .FirstOrDefault(u => u.UporabniskoIme == UporabniskoIme && u.Geslo == Geslo);

            if (user == null)
            {
                Sporocilo = "Napačno uporabniško ime ali geslo.";
                return Page();
            }

            _dataStore.TrenutniUporabnik = user;
            _dataStore.Aktivnosti.Add($"{user.UporabniskoIme} se je prijavil.");

            return RedirectToPage("/Uporabnik/Profile");
        }
    }
}
