
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

        public List<Node> Nodes { get; set; }

        private HashSet<string> _openNodes { get; set; }

        public Path(Node initialNode, int timeLeft)
        {
            TimeLeft = timeLeft;
            ReleasePressure = 0;
            Nodes = new(timeLeft) { initialNode };
            _openNodes = new();
        }

        public Path(Path path)
        {
            ReleasePressure = path.ReleasePressure;
            TimeLeft = path.TimeLeft;
            Nodes = path.Nodes.ToList();
            _openNodes = new(path._openNodes);
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
            _openNodes = new(path._openNodes);
            GotoNode(node);
        }

        public List<Node> Expand() => Nodes.Last().Children;

        public List<Node> Expand(Dictionary<string, int> previousHighestValues)
        {
            var result = new List<Node>();
            foreach (var childCandidate in Nodes.Last().Children)
            {
                if (Nodes.Contains(childCandidate))
                {
                    if (ReleasePressure > previousHighestValues[childCandidate.Id])
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
            Nodes.Add(node);
        }

        public bool ShouldBeConsideredForOpening() => Nodes.Last().Value > 0;

        public bool IsOpen() => _openNodes.Contains(Nodes.Last().Id);

        public void OpenValve()
        {
            --TimeLeft;
            _openNodes.Add(Nodes.Last().Id);
            ReleasePressure += TimeLeft * Nodes.Last().Value;
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
        List<(int ReleasedPressure, Path Path)> solutions = new();
        Dictionary<string, int> highestValueDictionary = new();

        Stack<Path> stack = new();
        stack.Push(new(_input[0], 30));

        int index = 0;
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (current.TimeLeft == 0)
            {
                solutions.Add((current.ReleasePressure, current));
                continue;
            }

            if (current.ShouldBeConsideredForOpening() && !current.IsOpen())
            {
                var path = new Path(current);
                path.OpenValve();
                stack.Push(path);
            }

            foreach (var child in current.Expand(highestValueDictionary))
            {
                var path = new Path(current, child);
                stack.Push(path);
            }

            if (highestValueDictionary.TryGetValue(current.Nodes.Last().Id, out var existingHighestValue))
            {
                if (current.ReleasePressure > existingHighestValue)
                {
                    highestValueDictionary[current.Nodes.Last().Id] = current.ReleasePressure;
                }
            }
            else
            {
                highestValueDictionary[current.Nodes.Last().Id] = current.ReleasePressure;
            }
            ++index;

            if (index % 250_000 == 0)
            {
                Console.WriteLine($"\tTime: {0.001 * sw.ElapsedMilliseconds:F3}");
                Console.WriteLine($"\tIndex: {index}, stack: {stack.Count}, currentTimeLeft: {current.TimeLeft}");
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

    //public static ICollection<Node> DepthFirst(IEnumerable<Node> allNodes, Func<Node, bool> isSuccess)
    //{
    //    Stack<Node> stack = new(new[] { allNodes.First() });
    //    Dictionary<string, Node> expanded = new();              // Node.Id as Key

    //    Node current = stack.Peek();

    //    int index = 0;

    //    //var sw = new System.Diagnostics.Stopwatch();
    //    //sw.Start();
    //    while (stack.Count > 0)
    //    {
    //        current = stack.Pop();
    //        expanded.Add(current.Id, current);

    //        if (isSuccess(current))
    //        {
    //            break;
    //        }

    //        foreach (var child in current.Children)
    //        {
    //            if (expanded.ContainsKey(child.Id))
    //            {
    //                stack.Push(child);
    //            }
    //        }

    //        ++index;

    //        //if (index % 250_000 == 0)
    //        //{
    //        //    Console.WriteLine($"\tTime: {0.001 * sw.ElapsedMilliseconds:F3}");
    //        //    Console.WriteLine($"\tIndex: {index}, queue: {queue.Count}");
    //        //}
    //    }

    //    List<Node> solution = new();
    //    var node = current;
    //    while (node.ParentKey != string.Empty)
    //    {
    //        solution.Add(node);
    //        node = expanded[current.ParentKey];
    //    }

    //    return solution;
    //}

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
