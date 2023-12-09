using FluentAssertions;
using Xunit;

namespace AoC23;

public class Day02
{
    private const string Example = """
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
""";

    [Fact]
    public void Test_ParseGame()
    {
        var line = Example.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).First();
        ParseGame(line).Should().BeEquivalentTo(
            new
            {
                Id = 1,
                Revealed = new[]
                {
                    new { B = 3, R = 4, G = 0 },
                    new { B = 6, R = 1, G = 2 },
                    new { B = 0, R = 0, G = 2 },
                },
            });
    }

    private static Game ParseGame(string game)
    {
        var id = int.Parse(game.Split(':').First().Split(" ").Last());
        return new Game(id, GetReveals(game));
    }

    private static IEnumerable<Reveal> GetReveals(string game)
    {
        foreach (var reveal in game.Split(':').Last().Split(';', StringSplitOptions.TrimEntries))
        {
            var cubes = reveal.Split(',');
            var r = ParseCubeCountOfColor(cubes, "red");
            var g = ParseCubeCountOfColor(cubes, "green");
            var b = ParseCubeCountOfColor(cubes, "blue");
            yield return new Reveal(r, g, b);
        }
    }

    private static int ParseCubeCountOfColor(string[] cubes, string color)
    {
        if (cubes.FirstOrDefault(x => x.Contains(color))?.Replace(color, string.Empty) is { } s)
        {
            return int.Parse(s);
        }

        return 0;
    }

    private record Game(int Id, IEnumerable<Reveal> Revealed);

    private record Reveal(int R, int G, int B);
}