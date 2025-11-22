using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Books
{
    public class OrderByModel : PageModel
    {
        private readonly DataStore _dataStore;

        public OrderByModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public List<Knjiga> Knjige { get; set; } = new List<Knjiga>();
        
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "naslov";
        
        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "asc";
        
        [BindProperty(SupportsGet = true)]
        public string ZanrFilter { get; set; } = "vsi";

        public List<SelectListItem> SortOptions { get; } = new()
        {
            new SelectListItem { Value = "naslov", Text = "Naslov" },
            new SelectListItem { Value = "avtor", Text = "Avtor" },
            new SelectListItem { Value = "zanr", Text = "Žanr" },
            new SelectListItem { Value = "leto", Text = "Leto izdaje" },
            new SelectListItem { Value = "rating", Text = "Ocena" },
            new SelectListItem { Value = "status", Text = "Status" }
        };

        public List<SelectListItem> OrderOptions { get; } = new()
        {
            new SelectListItem { Value = "asc", Text = "Naraščajoče" },
            new SelectListItem { Value = "desc", Text = "Padajoče" }
        };

        public List<SelectListItem> ZanrOptions { get; set; } = new();

        public void OnGet()
        {
            // Pridobi vse žanre za filter - POPRAVLJENO
            var vsiZanri = _dataStore.VsiZanri(); // Uporabi pravo ime metode
            ZanrOptions = vsiZanri
                .Select(z => new SelectListItem { Value = z, Text = z })
                .ToList();
            ZanrOptions.Insert(0, new SelectListItem { Value = "vsi", Text = "Vsi žanri" });

            // Pridobi in razvrsti knjige - POPRAVLJENO
            var vseKnjige = _dataStore.RazvrstiKnjige(SortBy, SortOrder);
            
            // Uporabi filter po žanru
            if (ZanrFilter != "vsi")
            {
                Knjige = vseKnjige.Where(k => k.Zanr == ZanrFilter).ToList();
            }
            else
            {
                Knjige = vseKnjige;
            }
        }
    }
}