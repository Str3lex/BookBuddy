using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly DataStore _dataStore;

        public DetailsModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Knjiga? Knjiga { get; set; }
        public List<Komentar> KomentarjiZaKnjigo { get; set; } = new List<Komentar>();
        public string? TrenutniUporabnikIme => _dataStore.TrenutniUporabnik?.UporabniskoIme;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public void OnGet()
        {
            Knjiga = _dataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (Knjiga != null)
            {
                KomentarjiZaKnjigo = _dataStore.Komentarji
                    .Where(k => k.Besedilo.Contains(Knjiga.Naslov) || _dataStore.Komentarji.Any())
                    .OrderByDescending(k => k.Datum)
                    .ToList();
            }
        }

        public IActionResult OnPostAddComment(int knjigaId, string besedilo)
        {
            if (_dataStore.TrenutniUporabnik == null)
                return RedirectToPage("/Auth/Login");

            if (string.IsNullOrWhiteSpace(besedilo) || besedilo.Length > 500)
            {
                TempData["Error"] = "Komentar mora vsebovati med 1 in 500 znakov.";
                return RedirectToPage(new { id = knjigaId });
            }

            var novKomentar = new Komentar
            {
                Uporabnik = _dataStore.TrenutniUporabnik.UporabniskoIme,
                Besedilo = besedilo.Trim(),
                Datum = DateTime.Now
            };

            _dataStore.Komentarji.Add(novKomentar);
            _dataStore.Aktivnosti.Add($"{_dataStore.TrenutniUporabnik.UporabniskoIme} je komentiral knjigo: {_dataStore.Knjige.FirstOrDefault(k => k.Id == knjigaId)?.Naslov}");

            return RedirectToPage(new { id = knjigaId });
        }
    }
}
