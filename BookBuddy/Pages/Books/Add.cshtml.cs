using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class AddModel : PageModel
    {
        [BindProperty] public string Naslov { get; set; }
        [BindProperty] public string Avtor { get; set; }
        [BindProperty] public int LetoIzdaje { get; set; }

        public string? Sporocilo { get; set; }

        public IActionResult OnPost()
        {
            var user = DataStore.TrenutniUporabnik;
            if (user == null)
                return RedirectToPage("/Auth/Login");

            var knjiga = new Knjiga
            {
                Id = DataStore.Knjige.Count + 1,
                Naslov = Naslov,
                Avtor = Avtor,
                Status = "Ni prebrana",
                Rate = 0,
            };

            // Če želiš tudi leto izdaje shranjevati:
            // knjiga.LetoIzidaje = LetoIzidaje;

            DataStore.Knjige.Add(knjiga);

            Sporocilo = "Knjiga uspešno dodana!";
            return Page();
        }
    }
}
