using System.Linq;

namespace AoC_2022;

public class Day_02 : BaseDay
{
    private enum Shape { Rock, Paper, Scissors }

    private readonly List<(Shape Opponent, Shape Me)> _input;

    public Day_02()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        static int ShapeScore(Shape shape)
        {
            return shape switch
            {
                Shape.Rock => 1,
                Shape.Paper => 2,
                Shape.Scissors => 3,
                _ => throw new()
            };
        }

        static int MatchScore(Shape opponent, Shape me)
        {
            var players = new[] { me, opponent };

            var winner = Math.Abs(opponent - me) switch
            {
                0 => -1,
                1 => (int)players.Max(),
                2 => (int)players.Min(),
                _ => throw new Exception(@"¯\_(ツ)_/¯")
            };

            return winner == -1
                ? 3
                : (winner == (int)opponent)
                    ? 0
                    : 6;
        }

        return new(_input
            .Sum(pair => ShapeScore(pair.Me) + MatchScore(pair.Opponent, pair.Me))
            .ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new($"");
    }

    private IEnumerable<(Shape, Shape)> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);

        while (!file.Empty)
        {
            var line = file.NextLine().ToSingleString();

            var opponent = line[0] switch
            {
                'A' => Shape.Rock,
                'B' => Shape.Paper,
                'C' => Shape.Scissors,
                _ => throw new()
            };
            var me = line[2] switch
            {
                'X' => Shape.Rock,
                'Y' => Shape.Paper,
                'Z' => Shape.Scissors,
                _ => throw new()
            };

            yield return (opponent, me);
        }
    }
}
