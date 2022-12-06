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
}
