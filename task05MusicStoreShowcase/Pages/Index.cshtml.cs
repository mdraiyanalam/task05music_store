using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicStore.DataGeneration.Generators;
using MusicStore.DataGeneration.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace task05MusicStoreShowcase.Pages;

public class IndexModel : PageModel
{
    private readonly SongGenerator _generator;
    private readonly AudioService _audioService;

    public List<SongFile> Songs { get; set; } = new();

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

    // AJAX Handler for dynamic updates
    public IActionResult OnPostLoadData()
    {
        LoadSongs();
        return Partial("_SongsTable", this);
    }

    private void LoadSongs(int pageSize = 20)
    {
        Songs.Clear();

        for (int i = 1; i <= pageSize; i++)
        {
            var song = _generator.Generate(i, AvgLikes);
            Songs.Add(new SongFile
            {
                Id = song.Id,
                Title = song.Title,
                Artist = song.Artist,
                Album = song.Album,
                Genre = song.Genre,
                Likes = song.Likes,
                FilePath = "" // Not used for generated data
            });
        }
    }

    public IActionResult OnGetPreview(int id)
    {
        var audioBytes = _audioService.GeneratePreviewAudio(id);
        if (audioBytes.Length == 0)
        {
            return Content("Audio file not found");
        }
        return File(audioBytes, "audio/mpeg", $"song_{id}.mp3");
    }
}

// Model for display
public class SongFile
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Likes { get; set; }
    public string FilePath { get; set; } = string.Empty;
}