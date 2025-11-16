using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Books
{
    public class ListModel : PageModel
    {
        public List<Knjiga> VseKnjige { get; set; } = new();
        public List<string>? IzbrisaneKnjige { get; set; }

        [BindProperty]
        public List<int> IzbraneKnjige { get; set; } = new();

        public void OnGet()
        {
            VseKnjige = DataStore.Knjige;
        }

        public IActionResult OnPostDelete(int id)
        {
            var knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == id);
            if (knjiga != null)
            {
                DataStore.Knjige.Remove(knjiga);
                // Brišemo tudi povezana mnenja
                var mnenjaZaBrisanje = DataStore.Mnenja.Where(m => m.KnjigaId == id).ToList();
                foreach (var mnenje in mnenjaZaBrisanje)
                {
                    DataStore.Mnenja.Remove(mnenje);
                }
            }
            return RedirectToPage();
        }

        public IActionResult OnPost()
        {
            if (IzbraneKnjige != null && IzbraneKnjige.Any())
            {
                IzbrisaneKnjige = new List<string>();

                foreach (var id in IzbraneKnjige)
                {
                    var knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == id);
                    if (knjiga != null)
                    {
                        IzbrisaneKnjige.Add(knjiga.Naslov);
                        DataStore.Knjige.Remove(knjiga);

                        // Brišemo tudi povezana mnenja
                        var mnenjaZaBrisanje = DataStore.Mnenja.Where(m => m.KnjigaId == id).ToList();
                        foreach (var mnenje in mnenjaZaBrisanje)
                        {
                            DataStore.Mnenja.Remove(mnenje);
                        }
                    }
                }
            }

            VseKnjige = DataStore.Knjige;
            return Page();
        }
    }
}