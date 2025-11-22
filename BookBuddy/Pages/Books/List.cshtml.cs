using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Models;
using BookBuddy.Services;
using System.Linq;

namespace BookBuddy.Pages.Books
{
    public class ListModel : PageModel
    {
        private readonly DataStore _dataStore;

        public ListModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public List<Knjiga> Knjige { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Iskanje { get; set; } = string.Empty;

        public void OnGet()
        {
            var knjige = _dataStore.Knjige.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Iskanje))
            {
                string s = Iskanje.ToLower();

                knjige = knjige.Where(k =>
                    k.Naslov.ToLower().Contains(s) ||
                    k.Avtor.ToLower().Contains(s) ||
                    k.Zanr.ToLower().Contains(s)
                );
            }

            // sortiranje po naslovu
            Knjige = knjige.OrderBy(k => k.Naslov).ToList();
        }
    }
}
