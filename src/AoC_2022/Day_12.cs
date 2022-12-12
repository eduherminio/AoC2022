using MoreLinq.Extensions;
using System.Text.RegularExpressions.Generated;
using Direction = SheepTools.Model.Direction;

namespace AoC_2022;

public class Day_12 : BaseDay
{
    private record Point(char Value, int X, int Y) : SheepTools.Model.IntPointWithValue<char>(Value, X, Y)
    {
        public List<Point> PossibleDestinationSquares { get; } = new List<Point>();
    }
    private List<List<Point>> _input;

    public Day_12()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        Point start = new(' ', 1, -1), end = new(' ', 1, -1);

        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input[y].Count; ++x)
            {
                var point = _input[y][x];

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
        }

        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input[y].Count; ++x)
            {
                var point = _input[y][x];

                foreach (var neighbour in new[] { point.Move(Direction.Up), point.Move(Direction.Down), point.Move(Direction.Left), point.Move(Direction.Right) })
                {
                    if (neighbour.X >= 0 && neighbour.Y >= 0 && neighbour.X < _input[0].Count && neighbour.Y < _input.Count)
                    {
                        var realNeighbour = _input[neighbour.Y][neighbour.X];
                        if (realNeighbour.Value - 1 <= point.Value)
                        {
                            point.PossibleDestinationSquares.Add(realNeighbour);
                        }
                    }
                }
            }
        }

        var flatInput = _input.SelectMany(l => l);
        Dictionary<Point, int> distanceToSource = new();
        Dictionary<Point, Point?> prevHopFromSource = new();
        var priorityQueue = new PriorityQueue<Point, int>(_input.Count * _input[0].Count);

        distanceToSource[start] = 0;
        foreach (var point in flatInput)
        {
            if (point != start)
            {
                distanceToSource[point] = int.MaxValue;
                prevHopFromSource[point] = null;
            }
            priorityQueue.Enqueue(point, distanceToSource[point]);
        }

        bool solutionFound = false;
        while (!solutionFound && priorityQueue.TryDequeue(out var node, out var priority))
        {
            foreach (var neighbour in node.PossibleDestinationSquares)
            {
                var distance = priority + 1;    // Distance between source and node + distance between neighbourd and node
                if (distance < distanceToSource[neighbour])
                {
                    distanceToSource[neighbour] = distance;
                    prevHopFromSource[neighbour] = node;
                    priorityQueue.Enqueue(neighbour, distance);

                    if (neighbour == end)
                    {
                        solutionFound = true;
                        break;
                    }
                }
            }
        }

        var path = new List<Point>();
        Point? current = end;
        while (current is not null)
        {
            if (!prevHopFromSource.TryGetValue(current, out var previous))
            {
                break;
            }
            path.Add(current);
            current = previous;
        }

        path.Reverse();

        return new($"{distanceToSource[end]}");
    }

    public override ValueTask<string> Solve_2()
    {
        _input = ParseInput().ToList();

        Point start = new(' ', 1, -1), end = new(' ', 1, -1);

        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input[y].Count; ++x)
            {
                var point = _input[y][x];

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
        }

        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input[y].Count; ++x)
            {
                var point = _input[y][x];

                foreach (var neighbour in new[] { point.Move(Direction.Up), point.Move(Direction.Down), point.Move(Direction.Left), point.Move(Direction.Right) })
                {
                    if (neighbour.X >= 0 && neighbour.Y >= 0 && neighbour.X < _input[0].Count && neighbour.Y < _input.Count)
                    {
                        var realNeighbour = _input[neighbour.Y][neighbour.X];
                        if (realNeighbour.Value - 1 <= point.Value)
                        {
                            point.PossibleDestinationSquares.Add(realNeighbour);
                        }
                    }
                }
            }
        }

        var flatInput = _input.SelectMany(l => l);
        var minDistace = int.MaxValue;

        foreach (var newStart in flatInput.Where(p => p.Value == 'a'))
        {
            Dictionary<Point, int> distanceToSource = new();
            Dictionary<Point, Point?> prevHopFromSource = new();
            var priorityQueue = new PriorityQueue<Point, int>(_input.Count * _input[0].Count);

            distanceToSource[newStart] = 0;
            foreach (var point in flatInput)
            {
                if (point != newStart)
                {
                    distanceToSource[point] = int.MaxValue;
                    prevHopFromSource[point] = null;
                }
                priorityQueue.Enqueue(point, distanceToSource[point]);
            }

            bool solutionFound = false;
            while (!solutionFound && priorityQueue.TryDequeue(out var node, out var priority))
            {
                foreach (var neighbour in node.PossibleDestinationSquares)
                {
                    var distance = priority + 1;    // Distance between source and node + distance between neighbourd and node
                    if (distance < distanceToSource[neighbour])
                    {
                        distanceToSource[neighbour] = distance;
                        prevHopFromSource[neighbour] = node;
                        priorityQueue.Enqueue(neighbour, distance);

                        if (neighbour == end)
                        {
                            solutionFound = true;
                            break;
                        }
                    }
                }
            }

            if (distanceToSource[end] < minDistace && distanceToSource[end] > 0)
            {
                minDistace = distanceToSource[end];
            }
        }

        return new($"{minDistace}");
    }

    private IEnumerable<List<Point>> ParseInput()
    {
        int y = 0;
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            yield return line.Select((ch, x) => new Point(ch, x, y)).ToList();
            ++y;
        }
    }
}
