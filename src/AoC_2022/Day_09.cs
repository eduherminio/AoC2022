using SheepTools.Model;

namespace AoC_2022;

public class Day_09 : BaseDay
{
    private readonly List<(Direction direction, int distance)> _input;

    public Day_09()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        static IntPoint GetCloser(IntPoint head, IntPoint tail)
        {
            Direction horizontal = head.X > tail.X ? Direction.Right : Direction.Left;
            Direction vertical = head.Y > tail.Y ? Direction.Up : Direction.Down;

            return tail.Move(horizontal).Move(vertical);
        }

        var tailSet = new HashSet<IntPoint>(_input.Count);
        var head = new IntPoint(0, 0);
        var tail = new IntPoint(0, 0);
        //Print(head, tail);

        foreach (var (direction, steps) in _input)
        {
            for (int step = 0; step < steps; step++)
            {
                head = head.Move(direction);
                var distance = tail.DistanceTo(head);

                tail = distance switch
                {
                    2 => tail.Move(direction),
                    > 1.42 => GetCloser(head, tail),    // Math.Sqrt(2) == 1.4142
                    _ => tail
                };
                tailSet.Add(tail);

                tailSet.Add(tail);
                //Print(head, tail);
            }
        }

        return new($"{tailSet.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
    }

    private void Print(IntPoint head, IntPoint tail)
    {
        var maxY = new[] { 0, head.Y, tail.Y }.Max() + 2;
        var maxX = new[] { 0, head.X, tail.X }.Max() + 2;

        var minY = new[] { 0, head.Y, tail.Y }.Min() - 1;
        var minX = new[] { 0, head.X, tail.X }.Min() - 1;

        for (int y = minY; y < maxY; ++y)
        {
            for (int x = minX; x < maxX; ++x)
            {
                if (x == head.X && y == head.Y)
                {
                    Console.Write("H");
                }
                else if (x == tail.X && y == tail.Y)
                {
                    Console.Write("T");
                }
                else if (x == 0 && y == 0)
                {
                    Console.Write('0');
                }
                else
                {
                    Console.Write("-");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("##################");
    }

    private IEnumerable<(Direction, int)> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);
        while (!file.Empty)
        {
            var line = file.NextLine();
            yield return new(ParseDirection(line.NextElement<string>()), line.NextElement<int>());
        }

        static Direction ParseDirection(string input) => input switch
        {
            "D" => Direction.Up,        // Visualization purposes
            "U" => Direction.Down,      // Visualization purposes
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => throw new()
        };
    }
}
