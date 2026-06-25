using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicStore.DataGeneration.Generators;
using MusicStore.DataGeneration.Models;

namespace task05MusicStoreShowcase.Pages;

public class TestModel : PageModel
{
    private readonly SongGenerator _generator;

    public List<Song> Songs { get; set; } = new();

    public TestModel(SongGenerator generator)
    {
        _generator = generator;
    }

    public void OnGet()
    {
        Songs = new List<Song>();
        for (int i = 1; i <= 10; i++)
        {
            Songs.Add(_generator.Generate(i, 3.5));
        }
    }
}