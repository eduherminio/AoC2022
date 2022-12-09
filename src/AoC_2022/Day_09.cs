using SheepTools.Model;

namespace AoC_2022;

public static class IntPointExtensions
{
    public static int ChebyshevDistance(this IntPoint point, IntPoint otherPoint)
    {
        var xDelta = Math.Abs(point.X - otherPoint.X);
        var yDelta = Math.Abs(point.Y - otherPoint.Y);

        return xDelta >= yDelta
            ? xDelta
            : yDelta;
    }
}

public class Day_09 : BaseDay
{
    private readonly List<(Direction direction, int distance)> _input;

    public Day_09()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1() => Solve_1_ChebyshevDistance();
    public override ValueTask<string> Solve_2() => Solve_2_ChebyshevDistance();

    private static int GenericSolve_Distance(List<(Direction direction, int distance)> input, int knotsCount)
    {
        var tailSet = new HashSet<IntPoint>(input.Count);

        var head = new IntPoint(0, 0);
        var knots = new List<IntPoint>(knotsCount);
        for (int i = 0; i < knotsCount; ++i) { knots.Add(new(0, 0)); }

        //Print(head, knots);

        foreach (var (direction, steps) in input)
        {
            for (int step = 0; step < steps; step++)
            {
                head = head.Move(direction);

                var reference = head;
                for (int knotIndex = 0; knotIndex < knots.Count; ++knotIndex)
                {
                    var knot = knots[knotIndex];
                    var distance = knot.DistanceTo(reference);

                    knot = distance switch
                    {
                        2 => GetCloserVerticallyOrHorizontally(reference, knot),
                        > 1.42 => GetCloserDiagonally(reference, knot),    // Math.Sqrt(2) == 1.4142
                        _ => knot
                    };
                    knots[knotIndex] = knot;

                    reference = knot;
                }
                tailSet.Add(knots[knotsCount - 1]);
                ////Print(head, knots);
            }
            //Print(head, knots);
        }

        return tailSet.Count;
    }

    private static int GenericSolve_ChebyshevDistance(List<(Direction direction, int distance)> input, int knotsCount)
    {
        var tailSet = new HashSet<IntPoint>(input.Count);

        var head = new IntPoint(0, 0);
        var knots = new List<IntPoint>(knotsCount);
        for (int i = 0; i < knotsCount; ++i) { knots.Add(new(0, 0)); }

        //Print(head, knots);

        foreach (var (direction, steps) in input)
        {
            for (int step = 0; step < steps; step++)
            {
                head = head.Move(direction);

                var reference = head;
                for (int knotIndex = 0; knotIndex < knots.Count; ++knotIndex)
                {
                    var knot = knots[knotIndex];
                    var distance = knot.ChebyshevDistance(reference);

                    if (distance == 2)
                    {
                        knots[knotIndex] = knot = GetCloser(reference, knot);
                    }

                    reference = knot;
                }
                tailSet.Add(knots[knotsCount - 1]);
                ////Print(head, knots);
            }
            //Print(head, knots);
        }

        return tailSet.Count;
    }

    private static int GenericSolve_ManhattanDistance(List<(Direction direction, int distance)> input, int knotsCount)
    {
        var tailSet = new HashSet<IntPoint>(input.Count);

        var head = new IntPoint(0, 0);
        var knots = new List<IntPoint>(knotsCount);
        for (int i = 0; i < knotsCount; ++i) { knots.Add(new(0, 0)); }

        //Print(head, knots);

        foreach (var (direction, steps) in input)
        {
            for (int step = 0; step < steps; step++)
            {
                head = head.Move(direction);

                var reference = head;
                for (int knotIndex = 0; knotIndex < knots.Count; ++knotIndex)
                {
                    var knot = knots[knotIndex];
                    var distance = knot.ManhattanDistance(reference);

                    knot = distance switch
                    {
                        2 => reference.X == knot.X
                            ? GetCloserVertically(reference, knot)
                            : (reference.Y == knot.Y
                                ? GetCloserHorizontally(reference, knot)
                                : knot),
                        >= 3 => GetCloserDiagonally(reference, knot),
                        _ => knot
                    };

                    knots[knotIndex] = knot;

                    reference = knot;
                }
                tailSet.Add(knots[knotsCount - 1]);
                ////Print(head, knots);
            }
            //Print(head, knots);
        }

        return tailSet.Count;
    }

    private static IntPoint GetCloserDiagonally(IntPoint reference, IntPoint tail)
    {
        Direction horizontal = reference.X > tail.X ? Direction.Right : Direction.Left;
        Direction vertical = reference.Y > tail.Y ? Direction.Up : Direction.Down;

        return tail.Move(horizontal).Move(vertical);
    }

    /// <summary>
    /// When following a reference instead of free-will movement (reference != head) and distance == 2,
    /// movement's direction isn't necessarily head's direction
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="tail"></param>
    /// <returns></returns>
    private static IntPoint GetCloserVerticallyOrHorizontally(IntPoint reference, IntPoint tail)
    {
        if (reference.X == tail.X)
        {
            return GetCloserVertically(reference, tail);
        }

        return GetCloserHorizontally(reference, tail);
    }

    private static IntPoint GetCloserVertically(IntPoint reference, IntPoint tail)
    {
        return tail.Move(reference.Y > tail.Y ? Direction.Up : Direction.Down);
    }

    private static IntPoint GetCloserHorizontally(IntPoint reference, IntPoint tail)
    {
        return tail.Move(reference.X > tail.X ? Direction.Right : Direction.Left);
    }

    private static IntPoint GetCloser(IntPoint reference, IntPoint tail)
    {
        if (reference.X == tail.X)
        {
            return tail.Move(reference.Y > tail.Y ? Direction.Up : Direction.Down);
        }
        if (reference.Y == tail.Y)
        {
            return tail.Move(reference.X > tail.X ? Direction.Right : Direction.Left);
        }

        Direction horizontal = reference.X > tail.X ? Direction.Right : Direction.Left;
        Direction vertical = reference.Y > tail.Y ? Direction.Up : Direction.Down;

        return tail.Move(horizontal).Move(vertical);
    }

    public ValueTask<string> Solve_1_Distance() => new($"{GenericSolve_Distance(_input, 1)}");
    public ValueTask<string> Solve_2_Distance() => new($"{GenericSolve_Distance(_input, 9)}");

    public ValueTask<string> Solve_1_ChebyshevDistance() => new($"{GenericSolve_ChebyshevDistance(_input, 1)}");
    public ValueTask<string> Solve_2_ChebyshevDistance() => new($"{GenericSolve_ChebyshevDistance(_input, 9)}");

    public ValueTask<string> Solve_1_ManhattanDistance() => new($"{GenericSolve_ManhattanDistance(_input, 1)}");
    public ValueTask<string> Solve_2_ManhattanDistance() => new($"{GenericSolve_ManhattanDistance(_input, 9)}");

    public ValueTask<string> Solve_1_Original()
    {
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
                    > 1.42 => GetCloserDiagonally(head, tail),    // Math.Sqrt(2) == 1.4142
                    _ => tail
                };
                tailSet.Add(tail);
                //Print(head, tail);
            }
        }

        return new($"{tailSet.Count}");
    }
    public ValueTask<string> Solve_2_Original()
    {
        var tailSet = new HashSet<IntPoint>(_input.Count);
        var head = new IntPoint(0, 0);
        var knots = new List<IntPoint>(9);
        for (int i = 0; i < 9; ++i) { knots.Add(new(0, 0)); }

        //Print(head, knots);

        foreach (var (direction, steps) in _input)
        {
            for (int step = 0; step < steps; step++)
            {
                head = head.Move(direction);

                var reference = head;
                for (int knotIndex = 0; knotIndex < knots.Count; ++knotIndex)
                {
                    var knot = knots[knotIndex];
                    var distance = knot.DistanceTo(reference);

                    knot = distance switch
                    {
                        2 => GetCloserVerticallyOrHorizontally(knot, reference),
                        > 1.42 => GetCloserDiagonally(reference, knot),    // Math.Sqrt(2) == 1.4142
                        _ => knot
                    };
                    knots[knotIndex] = knot;

                    reference = knot;
                }
                tailSet.Add(knots[8]);
                ////Print(head, knots);
            }
            //Print(head, knots);
        }

        return new($"{tailSet.Count}");
    }

    private static void Print(IntPoint head, IntPoint tail)
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

    private static void Print(IntPoint head, List<IntPoint> knots)
    {
        var maxY = knots.Select(k => k.Y).Append(0).Append(head.Y).Max() + 2;
        var maxX = knots.Select(k => k.X).Append(0).Append(head.X).Max() + 2;

        var minY = knots.Select(k => k.Y).Append(0).Append(head.Y).Min() - 1;
        var minX = knots.Select(k => k.X).Append(0).Append(head.X).Min() - 1;

        for (int y = minY; y < maxY; ++y)
        {
            for (int x = minX; x < maxX; ++x)
            {
                if (x == head.X && y == head.Y)
                {
                    Console.Write("H");
                }
                else
                {
                    bool foundKnot = false;
                    for (int i = 0; i < knots.Count; ++i)
                    {
                        if (knots[i].X == x && knots[i].Y == y)
                        {
                            Console.Write(i + 1);
                            foundKnot = true;
                            break;
                        }
                    }
                    if (!foundKnot)
                    {
                        if (x == 0 && y == 0)
                        {
                            Console.Write('0');
                        }
                        else
                        {
                            Console.Write("-");
                        }
                    }
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
