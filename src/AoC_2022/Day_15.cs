using System.Text.RegularExpressions;
using System.Linq;

namespace AoC_2022;

public partial class Day_15 : BaseDay
{
    [GeneratedRegex(@"Sensor at x=(?<sensorX>-?\d*), y=(?<sensorY>-?\d*): closest beacon is at x=(?<beaconX>-?\d*), y=(?<beaconY>-?\d*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex ParsingRegex();

    private sealed record Point(char Value, int X, int Y, Point? ClosestBeacon = null) : SheepTools.Model.IntPoint(X, Y)
    {
        public bool Equals(Point? other)
        {
            if (other is null)
            {
                return false;
            }

            return X == other.X && Y == other.Y;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    private readonly List<Point> _input;

    public Day_15()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        const int requestedY = 2_000_000;
        HashSet<Point> notBeacons = new();
        foreach (var sensor in _input)
        {
            var distance = (int)sensor.ManhattanDistance(sensor.ClosestBeacon ?? throw new SolvingException());
            foreach (var y in Enumerable.Range(sensor.Y - distance, 2 * distance))
            {
                if (y == requestedY)
                {
                    foreach (var x in Enumerable.Range(sensor.X - distance, 2 * distance))
                    {
                        var candidatePoint = new Point('#', x, y);
                        if (sensor.ManhattanDistance(candidatePoint) <= distance)
                        {
                            notBeacons.Add(candidatePoint);
                        }
                    }
                }
            }
            //Print(notBeacons, _input);
        }

        var discardedBeacons = notBeacons.Except(_input.Select(s => s.ClosestBeacon!));
        int result = discardedBeacons.Count(p => p.Y == requestedY);

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        const int upperLimit = 4_000_000;

        //var beaconsAndSensors = _input.SelectMany(i => new[] { i, i.ClosestBeacon! }).ToHashSet();
        //var inputPair = _input.Select(sensor =>
        //{
        //    var distance = (int)sensor.ManhattanDistance(sensor.ClosestBeacon ?? throw new SolvingException());
        //    return (sensor, distance);
        //});

        //for (int y = 0; y < upperLimit + 1; ++y)
        //{
        //    Console.WriteLine(y);
        //    for (int x = 0; x < upperLimit + 1; ++x)
        //    {
        //        var p = new Point('#', x, y);
        //        if (!inputPair.Any(pair => pair.sensor.ManhattanDistance(p) <= pair.distance) && !beaconsAndSensors.Contains(p))
        //        {
        //            return new($"{x * 4_000_000L + y}");
        //        }
        //    }
        //}

        //throw new SolvingException();

        //System.Collections.Concurrent.hash
        HashSet<Point> notBeacons = new(_input.SelectMany(i => new[] { i, i.ClosestBeacon! }));
        foreach (var sensor in _input)
        {
            var distance = (int)sensor.ManhattanDistance(sensor.ClosestBeacon ?? throw new SolvingException());
            var minY = Math.Clamp(sensor.Y - distance, 0, upperLimit);
            var maxY = Math.Clamp(sensor.Y + distance, 0, upperLimit);
            for (int y = minY; y <= maxY; ++y)
            {
                Console.WriteLine(y);
                var minX = Math.Clamp(sensor.X - distance, 0, upperLimit);
                var maxX = Math.Clamp(sensor.X + distance, 0, upperLimit);
                for (int x = minX; x <= maxX; ++x)
                {
                    var candidatePoint = new Point('#', x, y);
                    if (sensor.ManhattanDistance(candidatePoint) <= distance)
                    {
                        notBeacons.Add(candidatePoint);
                        //const long limit = (long)upperLimit * upperLimit - 1;
                        //if (notBeacons.LongCount() >= limit)
                        //{
                        //    break;
                        //}
                        //Console.WriteLine(notBeacons.Count);
                    }
                }
            }
        }

        //Print(notBeacons, _input);
        for (int y = 0; y < upperLimit; ++y)
        {
            for (int x = 0; x < upperLimit; ++x)
            {
                if (notBeacons.Add(new Point('#', x, y)))
                {
                    return new($"{x * 4_000_000L + y}");
                }
            }
        }

        throw new SolvingException();
    }

    private static void Print(IEnumerable<Point> notBeacons, IEnumerable<Point> input)
    {
        var total = new HashSet<Point>(input.SelectMany(i => new[] { i, i.ClosestBeacon! }).Concat(notBeacons));
        Console.ReadKey();
        Console.Clear();
        var maxY = total.Max(k => k.Y) + 2;
        var maxX = total.Max(k => k.X) + 2;

        var minY = total.Min(k => k.Y) - 1;
        var minX = total.Min(k => k.X) - 1;

        for (int x = minX; x < maxX; ++x)
            Console.Write(x);
        Console.WriteLine();

        for (int y = minY; y < maxY; ++y)
        {
            Console.Write(y);
            for (int x = minX; x < maxX; ++x)
            {
                if (total.TryGetValue(new Point('@', x, y), out var existingPoint))
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

    private List<Point> ParseInput()
    {
        return ParsingRegex().Matches(File.ReadAllText(InputFilePath)).Cast<Match>()
            .Select(match => new Point('S', int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value),
                        new Point('B', int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))))
            .ToList();
    }
}
