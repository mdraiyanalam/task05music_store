using Melanchall.DryWetMidi.Core;
using System.IO;

namespace MusicStore.DataGeneration.Services;

public class AudioService
{
    private readonly long _seed;

    public AudioService(long seed)
    {
        _seed = seed;
    }

    public byte[] GeneratePreviewAudio(int songIndex, int durationSeconds = 15)
    {
        // TODO: Implement real MIDI later
        return new byte[0]; // Placeholder
    }
}