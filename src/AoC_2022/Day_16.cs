
using SheepTools.Extensions;
using SheepTools.Model;
using System.Collections.Specialized;
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

    [GeneratedRegex(@"Valve (?<valve>[A-Z]*) has flow rate=(?<flowRate>-?\d*); tunnels? leads? to valves? (?<children>.+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex ParsingRegex();

    private readonly List<Node> _input;

    public Day_16()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var valvesThatReleasePressure = _input.Count(v => v.Value > 0);

        List<(int ReleasedPressure, Path Path)> solutions = new();

        Queue<Path> stack = new();
        stack.Enqueue(new(_input[0], 30));

        int index = 0;
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

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

            if (current.OpenNodes.Count == valvesThatReleasePressure)
            {
                continue;
            }

            foreach (var child in current.Expand())
            {
                var path = new Path(current, child);
                stack.Enqueue(path);
            }

            ++index;

            if (current.ReleasePressure > maxPressure)
            {
                maxPressure = current.ReleasePressure;
                Console.WriteLine($"\tMax release pressure: {current.ReleasePressure}");
            }

            if (index % 1_000_000 == 0)
            {
                Console.WriteLine($"\tTime: {0.001 * sw.ElapsedMilliseconds:F3}");
                Console.WriteLine($"\tIndex: {index}, stack: {stack.Count}, currentTimeLeft: {current.TimeLeft}");
                Console.WriteLine($"\tCurrent release pressure: {current.ReleasePressure}");
            }
        }

        var result = solutions.OrderByDescending(pair => pair.ReleasedPressure).First().ReleasedPressure;

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
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
