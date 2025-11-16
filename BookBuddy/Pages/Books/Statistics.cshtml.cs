using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Books
{
    public class StatisticsModel : PageModel
    {
        private readonly DataStore _dataStore;

        public StatisticsModel(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public int SteviloPrebranihKnjig { get; set; }
        public double PovprecnaOcena { get; set; }
        public int VsehKnjig { get; set; }
        public int SteviloVBranju { get; set; }
        public int SteviloNeprebranih { get; set; }
        public List<Knjiga> VseKnjige { get; set; } = new();

        public void OnGet()
        {
            VseKnjige = _dataStore.Knjige;
            VsehKnjig = VseKnjige.Count;

            var prebraneKnjige = VseKnjige.Where(k => k.Status == "Prebrana").ToList();
            SteviloPrebranihKnjig = prebraneKnjige.Count;

            var knjigeVBranju = VseKnjige.Where(k => k.Status == "V branju").ToList();
            SteviloVBranju = knjigeVBranju.Count;

            var neprebraneKnjige = VseKnjige.Where(k => k.Status == "Ni prebrana").ToList();
            SteviloNeprebranih = neprebraneKnjige.Count;

            var ocenjeneKnjige = VseKnjige.Where(k => k.Rate > 0).ToList();
            PovprecnaOcena = ocenjeneKnjige.Any() ? Math.Round(ocenjeneKnjige.Average(k => k.Rate), 2) : 0;
        }
    }
}
