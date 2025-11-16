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
        public int VsehKnjig { get; set; }
        public int SteviloPrebranihKnjig { get; set; }
        public int SteviloVBranju { get; set; }
        public double PovprecnaOcena { get; set; }

        public void OnGet()
        {
            var vseKnjige = _dataStore.Knjige;
            VsehKnjig = vseKnjige.Count;

            var prebraneKnjige = vseKnjige.Where(k => k.Status == "Prebrana").ToList();
            SteviloPrebranihKnjig = prebraneKnjige.Count;

            var knjigeVBranju = vseKnjige.Where(k => k.Status == "V branju").ToList();
            SteviloVBranju = knjigeVBranju.Count;

            var ocenjeneKnjige = vseKnjige.Where(k => k.Rate > 0).ToList();
            PovprecnaOcena = ocenjeneKnjige.Any() ? Math.Round(ocenjeneKnjige.Average(k => k.Rate), 1) : 0;
        }
    }
}
