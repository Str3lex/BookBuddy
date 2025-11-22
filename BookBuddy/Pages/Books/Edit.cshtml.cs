using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookBuddy.Models;
using BookBuddy.Services;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly DataStore _dataStore;

        public EditModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public KnjigaViewModel Knjiga { get; set; } = new KnjigaViewModel();

        public string? Sporocilo { get; set; }
        public string? ErrorSporocilo { get; set; }

        public void OnGet()
        {
            var obstojecaKnjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (obstojecaKnjiga == null)
            {
                ErrorSporocilo = "Knjiga ni najdena!";
                return;
            }

            Knjiga = new KnjigaViewModel
            {
                Id = obstojecaKnjiga.Id,
                Naslov = obstojecaKnjiga.Naslov,
                Avtor = obstojecaKnjiga.Avtor,
                Zanr = obstojecaKnjiga.Zanr,
                LetoIzdaje = obstojecaKnjiga.LetoIzdaje,
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var obstojecaKnjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (obstojecaKnjiga == null)
            {
                ErrorSporocilo = "Knjiga ni najdena!";
                return Page();
            }

            // Posodobi knjigo
            var posodobljenaKnjiga = new Knjiga
            {
                Id = Knjiga.Id,
                Naslov = Knjiga.Naslov,
                Avtor = Knjiga.Avtor,
                Zanr = Knjiga.Zanr,
                LetoIzdaje = Knjiga.LetoIzdaje,
            };

            _dataStore.PosodobiKnjigo(posodobljenaKnjiga);
            Sporocilo = "Knjiga je bila uspešno posodobljena!";

            return Page();
        }

        public IActionResult OnPostDelete()
        {
            var obstojecaKnjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (obstojecaKnjiga == null)
            {
                ErrorSporocilo = "Knjiga ni najdena!";
                return Page();
            }

            _dataStore.IzbrisiKnjigo(Id);
            TempData["SuccessMessage"] = $"Knjiga '{obstojecaKnjiga.Naslov}' je bila uspešno izbrisana!";
            
            return RedirectToPage("/Books/List");
        }
    }

    public class KnjigaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov je obvezen")]
        [Display(Name = "Naslov knjige")]
        public string Naslov { get; set; } = string.Empty;

        [Required(ErrorMessage = "Avtor je obvezen")]
        [Display(Name = "Avtor")]
        public string Avtor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Žanr je obvezen")]
        [Display(Name = "Žanr")]
        public string Zanr { get; set; } = string.Empty;

        [Required(ErrorMessage = "Leto izdaje je obvezno")]
        [Range(1000, 2100, ErrorMessage = "Leto mora biti med 1000 in 2100")]
        [Display(Name = "Leto izdaje")]
        public int LetoIzdaje { get; set; } = DateTime.Now.Year;
    }
}