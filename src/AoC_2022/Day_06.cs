namespace AoC_2022;

public class Day_06 : BaseDay
{
    private readonly string _input;

    public Day_06()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        for (int i = 3; i < _input.Length; ++i)
        {
            var set = new HashSet<char> { _input[i - 3], _input[i - 2], _input[i - 1], _input[i] };
            if (set.Count == 4)
            {
                return new($"{i + 1}");
            }
        }

        throw new SolvingException();
    }

    public override ValueTask<string> Solve_2()
    {
        for (int i = 13; i < _input.Length; ++i)
        {
            var set = new HashSet<char> {
                _input[i - 13], _input[i - 12], _input[i - 11], _input[i - 10], _input[i - 9], _input[i - 8],_input[i - 7],
                _input[i - 6], _input[i - 5], _input[i - 4], _input[i - 3], _input[i - 2], _input[i - 1], _input[i]
            };

            if (set.Count == 14)
            {
                return new($"{i + 1}");
            }
        }

        throw new SolvingException();
    }

    public ValueTask<string> Solve_1_WithSpan() => new($"{SolveWithSpan(_input, 4)}");

    public ValueTask<string> Solve_2_WithSpan() => new($"{SolveWithSpan(_input, 14)}");

    private static int SolveWithSpan(string input, int window)
    {
        var span = input.AsSpan();
        for (int i = window - 1; i < input.Length; ++i)
        {
            if (span.Slice(i - window + 1, window).ToArray().Distinct().Count() == window)
            {
                return i + 1;
            }
        }

        throw new SolvingException();
    }
}
