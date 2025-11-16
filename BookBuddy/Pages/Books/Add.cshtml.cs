using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public string Naslov { get; set; } = "";

        [BindProperty]
        public string Avtor { get; set; } = "";

        [BindProperty]
        public int LetoIzdaje { get; set; }

        [BindProperty]
        public string Zvrst { get; set; } = "";

        [BindProperty]
        public string Opis { get; set; } = "";

        public string? Sporocilo { get; set; }
        public string? ErrorSporocilo { get; set; }

        public IActionResult OnPost()
        {
            // Preveri prazna polja
            if (string.IsNullOrWhiteSpace(Naslov) ||
                string.IsNullOrWhiteSpace(Avtor) ||
                string.IsNullOrWhiteSpace(Zvrst) ||
                LetoIzdaje == 0)
            {
                ErrorSporocilo = "Vsa obvezna polja morajo biti izpolnjena!";
                return Page();
            }

            var knjiga = new Knjiga
            {
                Id = DataStore.Knjige.Count + 1,
                Naslov = Naslov.Trim(),
                Avtor = Avtor.Trim(),
                LetoIzdaje = LetoIzdaje,
                Zvrst = Zvrst.Trim(),
                Opis = Opis?.Trim() ?? "",
                Status = "Ni prebrana",
                Rate = 0
            };

            DataStore.Knjige.Add(knjiga);
            Sporocilo = "Knjiga uspešno dodana!";

            // Počisti polja
            Naslov = "";
            Avtor = "";
            Zvrst = "";
            Opis = "";
            LetoIzdaje = 0;

            return Page();
        }
    }
}
