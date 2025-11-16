using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class DetailsModel : PageModel
    {
        public Knjiga? Knjiga { get; set; }
        public List<Mnenje> MnenjaZaKnjigo { get; set; } = new();
        public int TrenutniUporabnikId => DataStore.TrenutniUporabnik?.Id ?? 0;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public void OnGet()
        {
            Knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (Knjiga != null)
            {
                MnenjaZaKnjigo = DataStore.Mnenja
                    .Where(m => m.KnjigaId == Knjiga.Id)
                    .OrderByDescending(m => m.Datum)
                    .ToList();
            }
        }

        public IActionResult OnPostAddMnenje(int knjigaId, string besedilo)
        {
            if (DataStore.TrenutniUporabnik == null)
                return RedirectToPage("/Auth/Login");

            if (string.IsNullOrWhiteSpace(besedilo) || besedilo.Length > 500)
            {
                TempData["Error"] = "Mnenje mora vsebovati med 1 in 500 znakov.";
                return RedirectToPage(new { id = knjigaId });
            }

            var novoMnenje = new Mnenje
            {
                Id = DataStore.Mnenja.Count + 1,
                KnjigaId = knjigaId,
                UporabnikId = DataStore.TrenutniUporabnik.Id,
                Besedilo = besedilo.Trim(),
                Datum = DateTime.Now
            };

            DataStore.Mnenja.Add(novoMnenje);
            return RedirectToPage(new { id = knjigaId });
        }

        public IActionResult OnPostDeleteMnenje(int mnenjeId)
        {
            var mnenje = DataStore.Mnenja.FirstOrDefault(m => m.Id == mnenjeId);
            if (mnenje != null && mnenje.UporabnikId == DataStore.TrenutniUporabnik?.Id)
            {
                DataStore.Mnenja.Remove(mnenje);
            }
            return RedirectToPage(new { id = mnenje?.KnjigaId });
        }
    }
}