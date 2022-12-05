using MoreLinq;
using System.Text.RegularExpressions;

namespace AoC_2022;

public partial class Day_05 : BaseDay
{
    [GeneratedRegex("move (?<move>\\d+) from (?<from>\\d+) to (?<to>\\d+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex regex();

    private List<Stack<char>> _stacks;
    private readonly List<(int Move, int From, int To)> _moves;

    public Day_05()
    {
        var (Stacks, Moves) = ParseInput();
        _stacks = Stacks;
        _moves = Moves;
    }

    public override ValueTask<string> Solve_1()
    {
        foreach (var (Move, From, To) in _moves)
        {
            for (int i = 0; i < Move; ++i)
            {
                _stacks[To].Push(_stacks[From].Pop());
            }
        }

        return new(string.Join("", _stacks.Select(s => s.Pop())));
    }

    public override ValueTask<string> Solve_2()
    {
        var (Stacks, _) = ParseInput();
        _stacks = Stacks;

        foreach (var (Move, From, To) in _moves)
        {
            var itemsToMove = new List<char>(Move);
            for (int i = 0; i < Move; ++i)
            {
                itemsToMove.Add(_stacks[From].Pop());
            }
            for (int i = Move - 1; i >= 0; --i)
            {
                _stacks[To].Push(itemsToMove[i]);
            }
        }

        return new(string.Join("", _stacks.Select(s => s.Pop())));
    }

    private (List<Stack<char>> Stacks, List<(int, int, int)> Moves) ParseInput(bool parseMoves = true)
    {
        var allLines = File.ReadLines(InputFilePath).ToList();

        int numberLineIndex = allLines.FindIndex(l => l[1] == '1');
        string numberLine = allLines[numberLineIndex];
        var numberOfStacks = int.Parse(numberLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        var charList = new List<List<char>>(numberOfStacks);
        for (int i = 0; i < numberOfStacks; ++i)
        {
            charList.Add(new(numberOfStacks * numberLineIndex));
        }

        int lineIndex = 0;
        var line = allLines[lineIndex];
        while (line[1] != '1')
        {
            int chunkIndex = 0;
            foreach (var chunk in line.Chunk(4))
            {
                var stack = charList[chunkIndex++];
                var letter = chunk[1];
                if (letter != ' ')
                {
                    stack.Add(letter);
                }
            }
            line = allLines[++lineIndex];
        }

        var charStack = charList.ConvertAll(l => new Stack<char>(l.Reverse<char>()));

        if (!parseMoves)
        {
            return (charStack, _moves);
        }

        var moves = new List<(int, int, int)>(allLines.Count - charList.Count - 1);

        for (int i = lineIndex + 2; i < allLines.Count; ++i)
        {
            foreach (var match in regex().Matches(allLines[i]).Cast<Match>())
            {
                moves.Add((
                    int.Parse(match.Groups["move"].Value),
                    int.Parse(match.Groups["from"].Value) - 1,
                    int.Parse(match.Groups["to"].Value) - 1));
            }
        }

        return (charStack, moves);
    }
}
