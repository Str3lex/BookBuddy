using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;
using System.Linq;

namespace BookBuddy.Pages.Books;

public class ListModel : PageModel
{
    public List<Knjiga> Knjige { get; set; }

    public void OnGet()
    {
        Knjige = DataStore.Knjige.ToList();
    }
}
