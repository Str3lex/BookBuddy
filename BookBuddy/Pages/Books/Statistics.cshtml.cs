using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;

namespace BookBuddy.Pages.Books
{
    public class StatisticsModel : PageModel
    {
        public int SteviloPrebranihKnjig { get; set; }
        public double PovprecnaOcena { get; set; }
        public int VsehKnjig { get; set; }
        public List<Knjiga> VseKnjige { get; set; } = new();

        public void OnGet()
        {
            VseKnjige = DataStore.Knjige;
            VsehKnjig = VseKnjige.Count;

            var prebraneKnjige = VseKnjige.Where(k => k.Status == "Prebrana").ToList();
            SteviloPrebranihKnjig = prebraneKnjige.Count;

            var ocenjeneKnjige = prebraneKnjige.Where(k => k.Rate > 0).ToList();
            PovprecnaOcena = ocenjeneKnjige.Any() ? ocenjeneKnjige.Average(k => k.Rate) : 0;
        }
    }
}