using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;

namespace BookBuddy.Pages.Books
{
    public class StatusModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string Status { get; set; } = "";

        public Knjiga? Knjiga { get; set; }
        public string? Sporocilo { get; set; }

        public void OnGet()
        {
            Knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == Id);
        }

        public IActionResult OnPost()
        {
            var knjiga = DataStore.Knjige.FirstOrDefault(k => k.Id == Id);
            if (knjiga == null)
            {
                Sporocilo = "Knjiga ni najdena.";
                return Page();
            }

            knjiga.Status = Status;
            return RedirectToPage("/Books/List");
        }
    }
}
