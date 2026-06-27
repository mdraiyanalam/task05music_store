using System.IO;

namespace MusicStore.DataGeneration.Services;

public class AudioService
{
    public byte[] GeneratePreviewAudio(int songIndex)
    {
        string audioFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio");

        if (!Directory.Exists(audioFolder))
        {
            return new byte[0];
        }

        var audioFiles = Directory.GetFiles(audioFolder)
                                  .Where(f => f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                                              f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                                  .ToArray();

        if (audioFiles.Length == 0)
        {
            return new byte[0];
        }

        string selectedFile = audioFiles[songIndex % audioFiles.Length];

        try
        {
            return File.ReadAllBytes(selectedFile);
        }
        catch
        {
            return new byte[0];
        }
    }
}