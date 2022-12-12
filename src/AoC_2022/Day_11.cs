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

    public override ValueTask<string> Solve_1()
    {
        var input = ParseInput().ToList();

        return new($"{GenericSolve(input, 20, (newItem) => newItem / 3)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var input = ParseInput().ToList();
        var lcm = (long)SheepTools.Maths.LeastCommonMultiple(input.Select(m => (ulong)m.DivisibleByTest));

        return new($"{GenericSolve(input, 10_000, (newItem) => newItem % lcm)}");
    }

    public static double GenericSolve(List<Monkey> input, int rounds, Func<long, long> manageWorryLevel)
    {
        var inspectedItems = new int[input.Count];

        for (int round = 1; round <= rounds; ++round)
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
                    newItem = manageWorryLevel(newItem);
                    var nextMonkey = newItem % monkey.DivisibleByTest == 0 ? monkey.TrueMonkey : monkey.FalseMonkey;

                    input[nextMonkey].Items.Add(newItem);
                }
                monkey.Items.Clear();
            }
        }

        return inspectedItems.OrderDescending().Take(2).Aggregate(1d, (total, element) => total * element);
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
