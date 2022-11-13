namespace AoC_2022;

public class Day_01 : BaseDay
{
    private readonly List<string> _input;

    public Day_01()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var solution = string.Empty;

        return new("jey");
    }

    public override ValueTask<string> Solve_2()
    {
        var solution = string.Empty;

        return new(solution);
    }

    private List<string> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);

        return File.ReadAllLines(InputFilePath).ToList();
    }
}

