using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class RateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public int Ocena { get; set; }

        public Knjiga? Knjiga { get; set; }
        public string? Sporocilo { get; set; }

        public void OnGet()
        {
            // Najdi knjigo po ID
            Knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == Id);
        }

        public IActionResult OnPost()
        {
            // Najdi knjigo po ID
            var knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == Id);

            if (knjiga == null)
            {
                Sporocilo = "Knjiga ni najdena!";
                return Page();
            }

            // Posodobi oceno
            knjiga.Rate = Ocena;

            // Vrni na seznam
            return RedirectToPage("/Books/List");
        }
    }
}
