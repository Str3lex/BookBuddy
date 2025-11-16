using Microsoft.AspNetCore.Mvc.RazorPages;
using BookBuddy.Services;

namespace BookBuddy.Pages.Auth;

public class ActivityModel : PageModel
{
    private readonly DataStore _dataStore;

    public ActivityModel(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<string> Aktivnosti { get; set; } = new List<string>();

    public void OnGet()
    {
        Aktivnosti = _dataStore.Aktivnosti ?? new List<string>();
    }
}
