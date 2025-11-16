using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class RateModel : PageModel
    {
        private readonly DataStore _dataStore;

        public RateModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public int Ocena { get; set; } = 3;  // Default ocena

        public Knjiga? Knjiga { get; set; }
        public string? Sporocilo { get; set; }

        public void OnGet()
        {
            Knjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
        }

        public IActionResult OnPost()
        {
            var knjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);

            if (knjiga == null)
            {
                Sporocilo = "Knjiga ni najdena!";
                return Page();
            }

            // Preverite veljavnost ocene
            if (Ocena < 1 || Ocena > 5)
            {
                Sporocilo = "Ocena mora biti med 1 in 5!";
                return Page();
            }

            knjiga.Rate = Ocena;

            _dataStore.Aktivnosti.Add($"{_dataStore.TrenutniUporabnik?.UporabniskoIme ?? "Gost"} je ocenil knjigo '{knjiga.Naslov}' z oceno {Ocena}");

            return RedirectToPage("/Books/List");
        }
    }
}
