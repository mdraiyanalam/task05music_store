using Microsoft.VisualBasic;
using NAudio.Wave;
using System.IO;

namespace MusicStore.DataGeneration.Services;

public class AudioService
{
    private readonly long _seed;

    public AudioService(long seed)
    {
        _seed = seed;
    }

    public byte[] GeneratePreviewAudio(int songIndex, int durationSeconds = 10)
    {
        var random = new Random((int)(_seed + songIndex));
        int sampleRate = 44100;
        int totalSamples = sampleRate * durationSeconds;

        var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
        using var memoryStream = new MemoryStream();
        using var writer = new WaveFileWriter(memoryStream, waveFormat);

        for (int i = 0; i < totalSamples; i++)
        {
            double t = i / (double)sampleRate;
            double frequency = 220 + (random.NextDouble() * 440);
            double amplitude = Math.Sin(2 * Math.PI * frequency * t) * 0.3;
            writer.WriteSample((float)amplitude);
        }

        writer.Flush();
        return memoryStream.ToArray();
    }
}