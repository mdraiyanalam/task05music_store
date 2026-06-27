using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicStore.DataGeneration.Generators;
using MusicStore.DataGeneration.Models;
using MusicStore.DataGeneration.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace task05MusicStoreShowcase.Pages;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly SongGenerator _generator;
    private readonly AudioService _audioService;

    public List<Song> Songs { get; set; } = new();

    public string CurrentLanguage { get; set; } = "en";
    public long CurrentSeed { get; set; } = 1234567890123456789L;
    public double AvgLikes { get; set; } = 3.5;

    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }

    public IndexModel(SongGenerator generator, AudioService audioService)
    {
        _generator = generator;
        _audioService = audioService;
    }

    public void OnGet(int page = 1)
    {
        CurrentPage = page;
        LoadSongs();
    }

    public IActionResult OnPostLoadData(string CurrentLanguage, long CurrentSeed, double AvgLikes, int page = 1)
    {
        this.CurrentLanguage = CurrentLanguage ?? "en";
        this.CurrentSeed = CurrentSeed;
        this.AvgLikes = AvgLikes;
        CurrentPage = page;

        LoadSongs();
        return Partial("_SongsTable", this);
    }

    private void LoadSongs()
    {
        Songs.Clear();

        // Dynamically count audio files
        string audioFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio");
        int audioFileCount = 0;

        if (Directory.Exists(audioFolder))
        {
            audioFileCount = Directory.GetFiles(audioFolder)
                .Count(f => f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                            f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase));
        }

        int totalSongsToGenerate = audioFileCount > 0 ? audioFileCount : 30;

        var allSongs = new List<Song>();
        for (int i = 1; i <= totalSongsToGenerate; i++)
        {
            allSongs.Add(_generator.Generate(i, AvgLikes));
        }

        // Pagination for Table View only
        TotalPages = (int)Math.Ceiling(allSongs.Count / (double)PageSize);

        // Table View gets paginated songs
        Songs = allSongs
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        // Store all songs for Gallery View (new property)
        AllSongsForGallery = allSongs;
    }

    public List<Song> AllSongsForGallery { get; set; } = new();

    public IActionResult OnGetPreview(int id)
    {
        var audioBytes = _audioService.GeneratePreviewAudio(id);
        if (audioBytes.Length == 0)
        {
            return Content("Audio preview not available");
        }
        return File(audioBytes, "audio/mpeg", $"song_{id}.mp3");
    }
}