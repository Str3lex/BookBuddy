using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class AddModel : PageModel
    {
        private readonly DataStore _dataStore;

        public AddModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty]
        public string Naslov { get; set; } = string.Empty;

        [BindProperty]
        public string Avtor { get; set; } = string.Empty;

        [BindProperty]
        public int LetoIzdaje { get; set; }

        [BindProperty]
        public string Zanr { get; set; } = string.Empty;

        public string? Sporocilo { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _dataStore.TrenutniUporabnik;
            if (user == null)
                return RedirectToPage("/Auth/Login");

            if (!ModelState.IsValid)
                return Page();

            var knjiga = new Knjiga
            {
                Id = _dataStore.Knjige.Count + 1,
                Naslov = Naslov,
                Avtor = Avtor,
                Zanr = string.IsNullOrWhiteSpace(Zanr) ? "Neopredeljeno" : Zanr,
                LetoIzdaje = LetoIzdaje,
                Status = "Ni prebrana",
                Rate = 0
            };

            _dataStore.SaveKnjiga(knjiga); // Use database save method
            _dataStore.Aktivnosti.Add($"{user.UporabniskoIme} je dodal knjigo: {Naslov}");

            Sporocilo = "Knjiga uspešno dodana!";
            return Page();
        }
    }
}
