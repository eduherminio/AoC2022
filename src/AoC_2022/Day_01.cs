namespace AoC_2022;

public class Day_01 : BaseDay
{
    private readonly List<int> _input;

    public Day_01()
    {
        _input = ParseInput()
            .Select(strList => strList.Select(str => int.Parse(str)))
            .Select(intList => intList.Sum())
            .ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{_input.Max()}");

    public override ValueTask<string> Solve_2() => new($"{_input.OrderByDescending(n => n).Take(3).Sum()}");

    private List<List<string>> ParseInput() => ParsedFile.ReadAllGroupsOfLines(InputFilePath);
}
