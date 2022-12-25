using SheepTools.Model;
using System.Text.RegularExpressions;

namespace AoC_2022;

public partial class Day_16 : BaseDay
{
    public record Node : GenericNode<string>
    {
        public int Value { get; set; }

        public List<Node> Children { get; set; } = new List<Node>();

        public Node(string id) : base(id) { }

        public Node(string id, int value, IEnumerable<Node> children) : base(id)
        {
            Value = value;
            Children.AddRange(children);
        }
    }

    public record Path
    {
        public int TimeLeft { get; private set; }

        public int ReleasePressure { get; private set; }

        public List<(Node Node, int ReleasePressure)> Nodes { get; private set; }

        public HashSet<string> OpenNodes { get; private set; }

        public Path(Node initialNode, int timeLeft)
        {
            TimeLeft = timeLeft;
            ReleasePressure = 0;
            Nodes = new(timeLeft) { (initialNode, ReleasePressure) };
            OpenNodes = new();
        }

        public Path(Path path)
        {
            ReleasePressure = path.ReleasePressure;
            TimeLeft = path.TimeLeft;
            Nodes = path.Nodes.ToList();
            OpenNodes = new(path.OpenNodes);
        }

        /// <summary>
        /// Includes <see cref="GotoNode(Node)"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        public Path(Path path, Node node)
        {
            ReleasePressure = path.ReleasePressure;
            TimeLeft = path.TimeLeft;
            Nodes = path.Nodes.ToList();
            OpenNodes = new(path.OpenNodes);
            GotoNode(node);
        }

        public List<Node> Expand()
        {
            var result = new List<Node>();
            foreach (var childCandidate in Nodes.Last().Node.Children)
            {
                var previousNodeVisit = Nodes.LastOrDefault(pair => pair.Node == childCandidate);
                if (previousNodeVisit != default)
                {
                    if (ReleasePressure > previousNodeVisit.ReleasePressure)
                    {
                        result.Add(childCandidate);
                    }
                }
                else
                {
                    result.Add(childCandidate);
                }
            }

            return result;
        }

        public void GotoNode(Node node)
        {
            --TimeLeft;
            Nodes.Add((node, ReleasePressure));
        }

        public bool ShouldBeConsideredForOpening() => Nodes.Last().Node.Value > 0;

        public bool IsOpen() => OpenNodes.Contains(Nodes.Last().Node.Id);

        public void OpenValve()
        {
            --TimeLeft;
            OpenNodes.Add(Nodes.Last().Node.Id);
            ReleasePressure += TimeLeft * Nodes.Last().Node.Value;

            var node = Nodes.Last().Node;
            Nodes.RemoveAt(Nodes.Count - 1);
            Nodes.Add((node, ReleasePressure));
        }
    }

    public record DoublePath
    {
        public int TimeLeft { get; private set; }
        public int ElephantTimeLeft { get; private set; }

        private int ReleasePressure;
        private int ElephantReleasePressure;

        public int TotalReleasePressure => ReleasePressure + ElephantReleasePressure;

        public List<(Node Node, int ReleasePressure)> Nodes { get; private set; }

        public List<(Node Node, int ElephantReleasePressure)> ElephantNodes { get; private set; }

        public HashSet<string> OpenNodes { get; private set; }

        public DoublePath(Node initialNode, int timeLeft, int elephantTimeLeft)
        {
            TimeLeft = timeLeft;
            ElephantTimeLeft = elephantTimeLeft;
            ReleasePressure = 0;
            ElephantReleasePressure = 0;
            Nodes = new(timeLeft) { (initialNode, ReleasePressure) };
            ElephantNodes = new(timeLeft) { (initialNode, ElephantReleasePressure) };
            OpenNodes = new();
        }

        public DoublePath(DoublePath path)
        {
            ReleasePressure = path.ReleasePressure;
            ElephantReleasePressure = path.ElephantReleasePressure;
            TimeLeft = path.TimeLeft;
            ElephantTimeLeft = path.ElephantTimeLeft;
            Nodes = path.Nodes.ToList();
            ElephantNodes = path.ElephantNodes.ToList();
            OpenNodes = new(path.OpenNodes);
        }

        public List<Node> Expand()
        {
            var result = new List<Node>();
            foreach (var childCandidate in Nodes.Last().Node.Children)
            {
                var previousNodeVisit = Nodes.LastOrDefault(pair => pair.Node == childCandidate);
                if (previousNodeVisit != default)
                {
                    if (ReleasePressure > previousNodeVisit.ReleasePressure)
                    {
                        result.Add(childCandidate);
                    }
                }
                else
                {
                    result.Add(childCandidate);
                }
            }

            return result;
        }

        public List<Node> ElephantExpand()
        {
            var result = new List<Node>();
            foreach (var childCandidate in ElephantNodes.Last().Node.Children)
            {
                var previousNodeVisit = ElephantNodes.LastOrDefault(pair => pair.Node == childCandidate);
                if (previousNodeVisit != default)
                {
                    if (ElephantReleasePressure > previousNodeVisit.ElephantReleasePressure)
                    {
                        result.Add(childCandidate);
                    }
                }
                else
                {
                    result.Add(childCandidate);
                }
            }

            return result;
        }

        public void GotoNode(Node node)
        {
            --TimeLeft;
            Nodes.Add((node, ReleasePressure));
        }

        public void ElephantGotoNode(Node node)
        {
            --ElephantTimeLeft;
            ElephantNodes.Add((node, ElephantReleasePressure));
        }

        public bool ShouldBeConsideredForOpening() => Nodes.Last().Node.Value > 0;
        public bool ElephantShouldBeConsideredForOpening() => ElephantNodes.Last().Node.Value > 0;

        public bool IsOpen() => OpenNodes.Contains(Nodes.Last().Node.Id);
        public bool ElephantIsOpen() => OpenNodes.Contains(ElephantNodes.Last().Node.Id);

        public void OpenValve()
        {
            --TimeLeft;
            OpenNodes.Add(Nodes.Last().Node.Id);
            ReleasePressure += TimeLeft * Nodes.Last().Node.Value;

            var node = Nodes.Last().Node;
            Nodes.RemoveAt(Nodes.Count - 1);
            Nodes.Add((node, ReleasePressure));
        }

        public void ElephantOpenValve()
        {
            --ElephantTimeLeft;
            OpenNodes.Add(ElephantNodes.Last().Node.Id);
            ElephantReleasePressure += ElephantTimeLeft * ElephantNodes.Last().Node.Value;

            var node = ElephantNodes.Last().Node;
            ElephantNodes.RemoveAt(ElephantNodes.Count - 1);
            ElephantNodes.Add((node, ElephantReleasePressure));
        }
    }

    [GeneratedRegex(@"Valve (?<valve>[A-Z]*) has flow rate=(?<flowRate>-?\d*); tunnels? leads? to valves? (?<children>.+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex ParsingRegex();

    private readonly List<Node> _input;

    public Day_16()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var valvesThatReleasePressure = _input.Where(v => v.Value > 0).OrderByDescending(n => n.Value).ToList();

        List<(int ReleasedPressure, Path Path)> solutions = new();

        Queue<Path> stack = new();
        stack.Enqueue(new(_input.First(n => n.Id == "AA"), 30));

        //int index = 0;
        //var sw = new System.Diagnostics.Stopwatch();
        //sw.Start();

        int maxPressure = int.MinValue;
        while (stack.Count > 0)
        {
            var current = stack.Dequeue();
            if (current.TimeLeft == 0)
            {
                solutions.Add((current.ReleasePressure, current));
                continue;
            }

            if (current.ShouldBeConsideredForOpening() && !current.IsOpen())
            {
                var path = new Path(current);
                path.OpenValve();
                stack.Enqueue(path);
            }

            if (current.ReleasePressure > maxPressure)
            {
                maxPressure = current.ReleasePressure;
                solutions.Add((current.ReleasePressure, current));
                //Console.WriteLine($"\t\tMax release pressure: {current.ReleasePressure}");
            }

            var valvesLeft = valvesThatReleasePressure.ExceptBy(current.OpenNodes, node => node.Id).Except(new[] { current.Nodes.Last().Node }).ToList();
            var potentialReleaseLeft = current.ReleasePressure;
            for (int i = 0; i < valvesLeft.Count; ++i)
            {
                potentialReleaseLeft += valvesLeft[i].Value * (current.TimeLeft - 2 * i);
            }

            if (potentialReleaseLeft < maxPressure)
            {
                solutions.Add((current.ReleasePressure, current));
                continue;
            }

            foreach (var child in current.Expand())
            {
                var path = new Path(current, child);
                stack.Enqueue(path);
            }

            //++index;

            //if (index % 1_000_000 == 0)
            //{
            //    Console.WriteLine($"\tTime: {0.001 * sw.ElapsedMilliseconds:F3}");
            //    Console.WriteLine($"\tIndex: {index}, stack: {stack.Count}, currentTimeLeft: {current.TimeLeft}");
            //    Console.WriteLine($"\tCurrent release pressure: {current.ReleasePressure}");
            //}
        }

        var result = solutions.OrderByDescending(pair => pair.ReleasedPressure).First().ReleasedPressure;

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<Node, Dictionary<Node, int>> distances = new(_input.Select(valve => KeyValuePair.Create(valve, Dijkstra(_input, valve))));

        var valvesThatReleasePressure = _input.Where(v => v.Value > 0).OrderByDescending(n => n.Value).ToList();

        List<(int TotalReleasedPressure, DoublePath Path)> solutions = new();

        Queue<DoublePath> stack = new();
        stack.Enqueue(new(_input.First(n => n.Id == "AA"), 26, 26));

        int index = 0;
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        int maxPressure = int.MinValue;
        while (stack.Count > 0)
        {
            var humanPathsToAdd = new List<DoublePath>();
            var current = stack.Dequeue();
            if (current.TimeLeft == 0)
            {
                solutions.Add((current.TotalReleasePressure, current));
                humanPathsToAdd.Add(current);
                goto elephants;
                //continue;
            }

            if (current.ShouldBeConsideredForOpening() && !current.IsOpen())
            {
                var path = new DoublePath(current);
                path.OpenValve();
                humanPathsToAdd.Add(path);
            }

            if (current.TotalReleasePressure > maxPressure)
            {
                maxPressure = current.TotalReleasePressure;
                solutions.Add((current.TotalReleasePressure, current));
                Console.WriteLine($"\t\tMax release pressure: {current.TotalReleasePressure}");
            }

            var valvesLeft = valvesThatReleasePressure.ExceptBy(current.OpenNodes, node => node.Id).Except(new[] { current.Nodes.Last().Node }).ToList();
            var potentialReleaseLeft = current.TotalReleasePressure;
            for (int i = 0; i < valvesLeft.Count; ++i)
            {
                var distance = distances[current.Nodes.Last().Node][valvesLeft[i]];
                var elephantDistance = distances[current.ElephantNodes.Last().Node][valvesLeft[i]];

                var min = new[] { distance, elephantDistance }.Min();
                potentialReleaseLeft += valvesLeft[i].Value * (current.TimeLeft - i - min);
            }

            //for (int i = 0; i < valvesLeft.Count / 2; i += 2)
            //{
            //    var distance = distances[current.Nodes.Last().Node][valvesLeft[i]];
            //    var elephantDistance = distances[current.ElephantNodes.Last().Node][valvesLeft[i]];

            //    if (distance > elephantDistance && i + 1 < valvesLeft.Count)
            //    {
            //        distance = distances[current.Nodes.Last().Node][valvesLeft[i + 1]];

            //        potentialReleaseLeft += valvesLeft[i + 1].Value * (current.TimeLeft - i - 1 - distance);
            //        potentialReleaseLeft += valvesLeft[i].Value * (current.ElephantTimeLeft - i - 1 - elephantDistance);
            //    }
            //    else if (i + 1 < valvesLeft.Count)
            //    {
            //        elephantDistance = distances[current.ElephantNodes.Last().Node][valvesLeft[i + 1]];

            //        potentialReleaseLeft += valvesLeft[i].Value * (current.TimeLeft - i - 1 - distance);
            //        potentialReleaseLeft += valvesLeft[i + 1].Value * (current.ElephantTimeLeft - i - 1 - elephantDistance);
            //    }

            //}

            //for (int i = 0; i < valvesLeft.Count; ++i)
            //{
            //    potentialReleaseLeft += valvesLeft[i].Value * (current.TimeLeft - 1 * i - distances[valvesLeft[i]].Values.Min());
            //}

            if (potentialReleaseLeft < maxPressure)
            {
                solutions.Add((current.TotalReleasePressure, current));
                continue;
            }

            foreach (var child in current.Expand())
            {
                var path = new DoublePath(current);
                path.GotoNode(child);
                humanPathsToAdd.Add(path);
            }

            elephants:

            if (current.ElephantTimeLeft == 0)
            {
                solutions.Add((current.TotalReleasePressure, current));
                if (humanPathsToAdd.Count != 1 || humanPathsToAdd[0] != current)
                {
                    foreach (var path in humanPathsToAdd)
                    {
                        stack.Enqueue(path);
                    }
                }
                continue;
            }

            if (current.ElephantShouldBeConsideredForOpening() && !current.ElephantIsOpen())
            {
                foreach (var path in humanPathsToAdd)
                {
                    if (path.Nodes.Last().Node != current.ElephantNodes.Last().Node || !path.IsOpen())     // Prevent both of them from opening the same valve at the same time
                    {
                        var newPath = new DoublePath(path);
                        newPath.ElephantOpenValve();
                        stack.Enqueue(newPath);
                    }
                }
            }

            if (current.TotalReleasePressure > maxPressure)
            {
                maxPressure = current.TotalReleasePressure;
                solutions.Add((current.TotalReleasePressure, current));
                Console.WriteLine($"\t\tMax release pressure: {current.TotalReleasePressure}");
            }

            var elephantChildren = current.ElephantExpand();
            foreach (var child in elephantChildren)
            {
                foreach (var path in humanPathsToAdd)
                {
                    var newPath = new DoublePath(path);
                    newPath.ElephantGotoNode(child);
                    stack.Enqueue(newPath);
                }
            }

            if (++index % 1_000_000 == 0)
            {
                Console.WriteLine($"\tTime: {0.001 * sw.ElapsedMilliseconds:F3}");
                Console.WriteLine($"\tIndex: {index}, stack: {stack.Count}, currentTimeLeft: {current.TimeLeft}");
                Console.WriteLine($"\tCurrent release pressure: {current.TotalReleasePressure}");
            }
        }

        var result = solutions.OrderByDescending(pair => pair.TotalReleasedPressure).First().TotalReleasedPressure;

        return new($"{result}");
    }

    private static Dictionary<Node, int> Dijkstra(List<Node> input, Node start, Node? end = null)
    {
        PriorityQueue<Node, int> priorityQueue = new(input.Count);
        Dictionary<Node, Node?> previousNode = new(input.Count);
        Dictionary<Node, int> distanceToSource = new(input.Count)
        {
            [start] = 0
        };
        const int maxDistance = 1_000_000_000;  // safe value that can be safely increased with node-neighbour distance

        foreach (var point in input)
        {
            if (point != start)
            {
                distanceToSource[point] = maxDistance;
                previousNode[point] = null;
            }

            priorityQueue.Enqueue(point, distanceToSource[point]);
        }

        while (priorityQueue.TryDequeue(out var node, out var priority))
        {
            foreach (var neighbour in node.Children)
            {
                var distance = priority + 1;    // Distance between source and node + distance between neighbourd and node
                if (distance < distanceToSource[neighbour])
                {
                    distanceToSource[neighbour] = distance;
                    priorityQueue.Enqueue(neighbour, distance);
                    previousNode[neighbour] = node;

                    if (neighbour == end)
                    {
                        return distanceToSource;
                    }
                }
            }
        }

        return distanceToSource;
    }

    private IEnumerable<Node> ParseInput()
    {
        Dictionary<string, Node> existingNodes = new();
        foreach (Match match in File.ReadAllLines(InputFilePath).Select(line => ParsingRegex().Match(line)))
        {
            var children = match.Groups[3].Value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(str =>
                {
                    if (existingNodes.TryGetValue(str, out var existingChild))
                    {
                        return existingChild;
                    }

                    return existingNodes[str] = new Node(str);
                });

            if (existingNodes.TryGetValue(match.Groups[1].Value, out var existingNode))
            {
                existingNode.Value = int.Parse(match.Groups[2].Value);
                existingNode.Children = children.ToList();
            }
            else
            {
                existingNode = existingNodes[match.Groups[1].Value] = new Node(match.Groups[1].Value, int.Parse(match.Groups[2].Value), children);
            }

            yield return existingNode;
        }
    }
}
