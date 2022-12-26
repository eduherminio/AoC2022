using System.Diagnostics;
using Direction = SheepTools.Model.Direction;

namespace AoC_2022;

public class Day_12 : BaseDay
{
    private sealed record Point(char Value, int X, int Y) : SheepTools.Model.IntPointWithValue<char>(Value, X, Y), IDijkstraNode<Point>
    {
        public IList<Point> Children { get; } = new List<Point>();
    }

    private readonly Point _start;
    private readonly Point _end;
    private readonly string[] _lines;
    private readonly List<Point> _input;

    public Day_12()
    {
        _lines = File.ReadAllLines(InputFilePath);
        _input = ParsePoints(_lines).ToList();

        (_start, _end) = ExtractStartAndEnd(_input);
        _input = AddPossibleDestinationSquares(_input, _lines[0].Length, _lines.Length);
    }

    public override ValueTask<string> Solve_1()
    {
        var input = AddPossibleDestinationSquares(_input, _lines[0].Length, _lines.Length);
        var distances = Helpers.Dijkstra(input, _start, _end);

        return new($"{distances[_end]}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = AddOppositePossibleDestinationSquares(_input, _lines[0].Length, _lines.Length);

        var distances = Helpers.Dijkstra(input, _end);
        var minDistance = distances.Where(pair => pair.Key.Value == 'a').Min(pair => pair.Value);

        return new($"{minDistance}");
    }

    private static IEnumerable<Point> ParsePoints(string[] lines)
    {
        int y = 0;
        foreach (var line in lines)
        {
            for (int x = 0; x < line.Length; ++x)
            {
                yield return new Point(line[x], x, y);
            }
            ++y;
        }
    }

    /// <summary>
    /// Also replaces start point value with 'a' and end point value with 'z'
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="SolvingException"></exception>
    private static (Point, Point) ExtractStartAndEnd(List<Point> input)
    {
        Point? start = default, end = default;

        foreach (var point in input)
        {
            if (point.Value == 'S')
            {
                point.Value = 'a';
                start = point;
            }
            else if (point.Value == 'E')
            {
                point.Value = 'z';
                end = point;
            }
        }

        return (start ?? throw new SolvingException(), end ?? throw new SolvingException());
    }

    private static List<Point> AddPossibleDestinationSquares(List<Point> input, int maxX, int maxY)
    {
        foreach (var point in input)
        {
            point.Children.Clear();
            foreach (var neighbour in new[] { point.Move(Direction.Up), point.Move(Direction.Down), point.Move(Direction.Left), point.Move(Direction.Right) })
            {
                if (neighbour.X >= 0 && neighbour.Y >= 0 && neighbour.X < maxX && neighbour.Y < maxY)
                {
                    var realNeighbour = input[(neighbour.Y * maxX) + neighbour.X];

                    Debug.Assert(realNeighbour.X == neighbour.X && realNeighbour.Y == neighbour.Y);

                    if (realNeighbour.Value - 1 <= point.Value)
                    {
                        point.Children.Add(realNeighbour);
                    }
                }
            }
        }

        return input;
    }

    private static List<Point> AddOppositePossibleDestinationSquares(List<Point> input, int maxX, int maxY)
    {
        foreach (var point in input)
        {
            point.Children.Clear();
            foreach (var neighbour in new[] { point.Move(Direction.Up), point.Move(Direction.Down), point.Move(Direction.Left), point.Move(Direction.Right) })
            {
                if (neighbour.X >= 0 && neighbour.Y >= 0 && neighbour.X < maxX && neighbour.Y < maxY)
                {
                    var realNeighbour = input[(neighbour.Y * maxX) + neighbour.X];

                    Debug.Assert(realNeighbour.X == neighbour.X && realNeighbour.Y == neighbour.Y);

                    if (realNeighbour.Value + 1 >= point.Value)
                    {
                        point.Children.Add(realNeighbour);
                    }
                }
            }
        }

        return input;
    }
}
