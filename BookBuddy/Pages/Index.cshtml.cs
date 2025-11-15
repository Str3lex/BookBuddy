using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;

namespace BookBuddy.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataStore _dataStore;

        public IndexModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void OnGet()
        {
            ViewData["IsLoggedIn"] = _dataStore.TrenutniUporabnik != null;
            ViewData["Username"] = _dataStore.TrenutniUporabnik?.UporabniskoIme;
        }
    }
}