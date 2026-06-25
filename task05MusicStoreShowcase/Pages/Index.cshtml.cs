using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicStore.DataGeneration.Generators;
using MusicStore.DataGeneration.Models;
using MusicStore.DataGeneration.Services;
using System.Collections.Generic;

namespace task05MusicStoreShowcase.Pages;

public class IndexModel : PageModel
{
    private readonly SongGenerator _generator;
    private readonly AudioService _audioService;

    public List<Song> Songs { get; set; } = new();

    [BindProperty]
    public string CurrentLanguage { get; set; } = "en";

    [BindProperty]
    public long CurrentSeed { get; set; } = 1234567890123456789L;

    [BindProperty]
    public double AvgLikes { get; set; } = 3.5;

    public IndexModel(SongGenerator generator, AudioService audioService)
    {
        _generator = generator;
        _audioService = audioService;
    }

    public void OnGet()
    {
        LoadSongs();
    }

    public void OnPost()
    {
        LoadSongs();
    }

    private void LoadSongs(int pageSize = 20)
    {
        Songs.Clear();
        for (int i = 1; i <= pageSize; i++)
        {
            Songs.Add(_generator.Generate(i, AvgLikes));
        }
    }

    public IActionResult OnGetPreview(int id)
    {
        var audioBytes = _audioService.GeneratePreviewAudio(id, 8); // 8 seconds preview
        return File(audioBytes, "audio/wav", $"preview_song_{id}.wav");
    }
}