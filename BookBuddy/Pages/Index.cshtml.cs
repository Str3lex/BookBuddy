using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataStore _dataStore;

        public IndexModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Models.Uporabnik? TrenutniUporabnik => _dataStore.TrenutniUporabnik;

        public void OnGet()
        {
        }
    }
}
