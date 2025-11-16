using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public string UporabniskoIme { get; set; } = "";
        [BindProperty] public string Eposta { get; set; } = "";
        [BindProperty] public string Geslo { get; set; } = "";

        public string? Sporocilo { get; set; }

        public IActionResult OnPost()
        {
            if (DataStore.Uporabniki.Any(u => u.UporabniskoIme == UporabniskoIme))
            {
                Sporocilo = "Uporabnik že obstaja!";
                return Page();
            }

            var user = new Uporabnik
            {
                Id = DataStore.Uporabniki.Count + 1,
                UporabniskoIme = UporabniskoIme,
                Eposta = Eposta,
                Geslo = Geslo
            };

            DataStore.Uporabniki.Add(user);
            DataStore.TrenutniUporabnik = user;

            return RedirectToPage("/User/Profile"); 
        }
    }
}