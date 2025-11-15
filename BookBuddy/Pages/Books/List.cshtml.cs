using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;
using BookBuddy.Models;
using System.Linq;

namespace BookBuddy.Pages.Books;

public class ListModel : PageModel
{
    private readonly DataStore _dataStore;

    public ListModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<Knjiga> Knjige { get; set; } = new List<Knjiga>();

    public void OnGet()
    {
        Knjige = _dataStore.Knjige.ToList();
    }
}
