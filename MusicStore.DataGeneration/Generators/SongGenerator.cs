using Bogus;
using MusicStore.DataGeneration.Models;

namespace MusicStore.DataGeneration.Generators;

public class SongGenerator
{
    private readonly int _seed;
    private readonly string _locale;

    public SongGenerator(long seed, string locale = "en")
    {
        _seed = (int)(seed % int.MaxValue);
        _locale = locale ?? "en";
    }

    public Song Generate(int index, double avgLikes)
    {
        Randomizer.Seed = new Random(_seed + index);
        var faker = new Faker<Song>(_locale);

        faker.RuleFor(s => s.Id, _ => index)
             .RuleFor(s => s.Title, f => GenerateSongTitle(f))
             .RuleFor(s => s.Artist, f => f.Name.FullName())
             .RuleFor(s => s.Album, f => f.Random.Bool(0.7f) ? f.Commerce.ProductName() : "Single")
             .RuleFor(s => s.Genre, f => f.Music.Genre())
             .RuleFor(s => s.Likes, _ => CalculateLikes(avgLikes));

        return faker.Generate();
    }

    private string GenerateSongTitle(Faker f)
    {
        var adjectives = new[] { "Dark", "Electric", "Midnight", "Golden", "Silent", "Broken", "Neon", "Crystal", "Lost", "Eternal" };
        var nouns = new[] { "Heart", "Dream", "Shadow", "Fire", "Rain", "Star", "Night", "Soul", "Echo", "Horizon" };
        return $"{adjectives[f.Random.Int(0, adjectives.Length - 1)]} {nouns[f.Random.Int(0, nouns.Length - 1)]}";
    }

    private int CalculateLikes(double avg)
    {
        return (int)Math.Floor(avg) + (Random.Shared.NextDouble() < (avg % 1) ? 1 : 0);
    }
}