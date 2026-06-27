using System.IO;

namespace MusicStore.DataGeneration.Services;

public class AudioService
{
    public byte[] GeneratePreviewAudio(int songIndex)
    {
        string audioFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio");

        Console.WriteLine("=== AudioService called for song " + songIndex + " ===");
        Console.WriteLine("Looking for audio in: " + audioFolder);

        if (!Directory.Exists(audioFolder))
        {
            Console.WriteLine("ERROR: Audio folder does not exist!");
            return new byte[0];
        }

        var audioFiles = Directory.GetFiles(audioFolder)
                                  .Where(f => f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                                              f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                                  .ToArray();

        Console.WriteLine("Found " + audioFiles.Length + " audio files");

        if (audioFiles.Length == 0)
        {
            Console.WriteLine("ERROR: No audio files found in folder!");
            return new byte[0];
        }

        string selectedFile = audioFiles[songIndex % audioFiles.Length];

        Console.WriteLine("Selected file: " + selectedFile);

        try
        {
            return File.ReadAllBytes(selectedFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR reading file: " + ex.Message);
            return new byte[0];
        }
    }
}