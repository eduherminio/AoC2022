using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AoC_2022;

public partial class Day_11 : BaseDay
{
    public record Monkey(int Number, List<long> Items, Func<long, long> Operation, int DivisibleByTest, int TrueMonkey, int FalseMonkey);

    [GeneratedRegex($@"Monkey (?<{nameof(_monkeyNumberRegex)}>\d+):", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex _monkeyNumberRegex();

    [GeneratedRegex($@"\s(?<{nameof(_startingItemRegex)}>\d+),?", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex _startingItemRegex();

    [GeneratedRegex($@"Operation: new = (?<{nameof(_operationRegex)}>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex _operationRegex();

    [GeneratedRegex($@"divisible by (?<{nameof(_testRegex)}>\d+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex _testRegex();

    [GeneratedRegex($@"If true: throw to monkey (?<{nameof(_trueTestResultRegex)}>\d+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex _trueTestResultRegex();

    [GeneratedRegex($@"If false: throw to monkey (?<{nameof(_falseTestResultRegex)}>\d+)", RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex _falseTestResultRegex();

    private readonly List<Monkey> _input;

    public Day_11()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var inspectedItems = new int[_input.Count];
        for (int round = 1; round <= 20; ++round)
        {
            for (int monkeyIndex = 0; monkeyIndex < _input.Count; ++monkeyIndex)
            {
                var monkey = _input[monkeyIndex];
                inspectedItems[monkeyIndex] += monkey.Items.Count;

                foreach (var item in monkey.Items)
                {
                    var newItem = monkey.Operation(item) / 3;
                    var nextMonkey = newItem % (long)monkey.DivisibleByTest == 0 ? monkey.TrueMonkey : monkey.FalseMonkey;
                    _input[nextMonkey].Items.Add(newItem);
                }
                monkey.Items.Clear();
            }
        }

        return new($"{inspectedItems.OrderDescending().Take(2).Aggregate(1, (total, element) => total * element)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = ParseInput().ToList();

        var inspectedItems = new int[input.Count];
        var lcm = (long)SheepTools.Maths.LeastCommonMultiple(input.Select(m => (ulong)m.DivisibleByTest));

        for (int round = 1; round <= 10_000; ++round)
        {
            //if (round % 100 == 0)
            //{
            //    Console.WriteLine(round);
            //}

            for (int monkeyIndex = 0; monkeyIndex < input.Count; ++monkeyIndex)
            {
                var monkey = input[monkeyIndex];
                inspectedItems[monkeyIndex] += monkey.Items.Count;

                foreach (var item in monkey.Items)
                {
                    var newItem = monkey.Operation(item);
                    var nextMonkey = newItem % monkey.DivisibleByTest == 0 ? monkey.TrueMonkey : monkey.FalseMonkey;

                    while (newItem > 2 * lcm)
                    {
                        newItem -= lcm;
                    }

                    input[nextMonkey].Items.Add(newItem);
                }
                monkey.Items.Clear();
            }
        }

        return new($"{inspectedItems.OrderDescending().Take(2).Aggregate(1d, (total, element) => total * element)}");
    }

    private IEnumerable<Monkey> ParseInput()
    {
        foreach (var group in ParsedFile.ReadAllGroupsOfLines(InputFilePath))
        {
            int monkeyNumber = int.Parse(_monkeyNumberRegex().Match(group[0]).Groups[nameof(_monkeyNumberRegex)].Value);
            List<long> startingItems = _startingItemRegex().Matches(group[1]).Select(match => long.Parse(match.Groups[nameof(_startingItemRegex)].Value)).ToList();
            string operationString = _operationRegex().Match(group[2]).Groups[nameof(_operationRegex)].Value;
            int divisibleBy = int.Parse(_testRegex().Match(group[3]).Groups[nameof(_testRegex)].Value);
            int monkeyToThrowIfTrue = int.Parse(_trueTestResultRegex().Match(group[4]).Groups[nameof(_trueTestResultRegex)].Value);
            int monkeyToThrowIfFalse = int.Parse(_falseTestResultRegex().Match(group[5]).Groups[nameof(_falseTestResultRegex)].Value);

            var operationItems = operationString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var isIntA = long.TryParse(operationItems[0], out long a);
            var isIntB = long.TryParse(operationItems[2], out long b);

            Func<long, long> op = operationItems[1] switch
            {
                "+" => (old) => (isIntA ? a : old) + (isIntB ? b : old),
                "-" => (old) => (isIntA ? a : old) - (isIntB ? b : old),
                "*" => (old) => (isIntA ? a : old) * (isIntB ? b : old),
                "/" => (old) => (isIntA ? a : old) / (isIntB ? b : old),
                _ => throw new()
            };

            yield return new Monkey(monkeyNumber, startingItems, op, divisibleBy, monkeyToThrowIfTrue, monkeyToThrowIfFalse);
        }
    }
}
