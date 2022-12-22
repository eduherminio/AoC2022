using SheepTools.Model;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC_2022;

public class Day_14 : BaseDay
{
    private sealed record Point(char Value, int X, int Y) : SheepTools.Model.IntPoint(X, Y)
    {
        public int State { get; set; }

        public new Point Move(Direction direction, int distance = 1)
        {
            switch (direction)
            {
                case Direction.Right:
                    return this with
                    {
                        X = X + distance
                    };
                case Direction.Left:
                    return this with
                    {
                        X = X - distance
                    };
                case Direction.Up:
                    return this with
                    {
                        Y = Y + distance
                    };
                case Direction.Down:
                    return this with
                    {
                        Y = Y - distance
                    };
                default:
                    {
                        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
                        defaultInterpolatedStringHandler.AppendLiteral("Direction ");
                        defaultInterpolatedStringHandler.AppendFormatted(direction);
                        defaultInterpolatedStringHandler.AppendLiteral(" isn't supported yet");
                        throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
                    }
            }
        }

        public Point GenerateAlternative()
        {
            var valueToReturn = State switch
            {
                0 => Move(Direction.Up),
                1 => Move(Direction.Up).Move(Direction.Left),
                2 => Move(Direction.Up).Move(Direction.Right),
                3 => this,
                _ => throw new SolvingException(),
            };

            ++State;
            return valueToReturn;
        }

        public bool Equals(Point? other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    private readonly HashSet<Point> _input;

    public Day_14()
    {
        _input = ParsedInput();
    }

    public override ValueTask<string> Solve_1()
    {
        Point UpdateSandGrain(Point originalSandGrain)
        {
            var newSandGrain = originalSandGrain.GenerateAlternative();
            if (originalSandGrain == newSandGrain)
            {
                return originalSandGrain;
            }

            if (_input.TryGetValue(newSandGrain, out var existingPoint))
            {
                var existingValue = existingPoint.Value;
                if (existingValue == '#' || existingValue == 'o')
                {
                    return UpdateSandGrain(originalSandGrain);
                }
            }

            return newSandGrain;
        }

        var generator = new Point('o', 500, 0);
        _input.Add(generator);
        var sand = new HashSet<Point>();

        var maxY = _input.Max(k => k.Y) + 2;
        while (true)
        {
            bool isExit = false;
            var existingSandGrain = new Point(generator.Value, generator.X, generator.Y);
            do
            {
                var newSandGrain = UpdateSandGrain(existingSandGrain);

                if (newSandGrain == existingSandGrain)
                {
                    isExit = true;
                    break;
                }
                else
                {
                    existingSandGrain = newSandGrain;
                    existingSandGrain.State = 0;
                }
            } while (existingSandGrain.Y <= maxY + 1);

            if (!isExit)
            {
                break;
            }
            _input.Add(existingSandGrain); // comment?
            sand.Add(existingSandGrain);

            //Print(_input);
        }

        return new($"{sand.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new($"");
    }

    private HashSet<Point>? ParsedInput()
    {
        var rocks = new HashSet<Point>();

        foreach (var line in File.ReadLines(InputFilePath))
        {
            var pointPairs = line.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            for (int i = 0; i < pointPairs.Length - 1; ++i)
            {
                var splitPair = pointPairs[i].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var start = new Point('#', int.Parse(splitPair[0]), int.Parse(splitPair[1]));
                splitPair = pointPairs[i + 1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var end = new Point('#', int.Parse(splitPair[0]), int.Parse(splitPair[1]));

                if (end.X < start.X || end.Y < start.Y)
                {
                    (start, end) = (end, start);
                }

                foreach (var x in Enumerable.Range(start.X, Math.Abs(end.X - start.X) + 1))
                {
                    foreach (var y in Enumerable.Range(start.Y, Math.Abs(end.Y - start.Y) + 1))
                    {
                        rocks.Add(new Point('#', x, y));
                    }
                }
            }
        }

        return rocks;
    }

    private static void Print(HashSet<Point> points)
    {
        Console.ReadKey();
        Console.Clear();
        var maxY = points.Where(p => p.Value == 'o').Max(k => k.Y) + 10;
        //var maxY = points.Max(k => k.Y) + 2;
        var maxX = points.Max(k => k.X) + 2;

        var minY = points.Min(k => k.Y) - 1;
        var minX = points.Min(k => k.X) - 1;

        for (int y = minY; y < maxY; ++y)
        {
            for (int x = minX; x < maxX; ++x)
            {
                points.TryGetValue(new Point('@', x, y), out var existingPoint);
                if (existingPoint is not null)
                {
                    Console.Write(existingPoint.Value);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
    }
}
