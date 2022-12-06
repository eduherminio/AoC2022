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

    public ValueTask<string> Solve_1_WithEnumerableRange() => new($"{SolveWithEnumerableRange(_input, 4)}");

    public ValueTask<string> Solve_2_WithEnumerableRange() => new($"{SolveWithEnumerableRange(_input, 14)}");

    public ValueTask<string> Solve_1_Distinct()
    {
        for (int i = 3; i < _input.Length; ++i)
        {
            var set = new[] { _input[i - 3], _input[i - 2], _input[i - 1], _input[i] };
            if (set.Distinct().Count() == 4)
            {
                return new($"{i + 1}");
            }
        }

        throw new SolvingException();
    }

    public ValueTask<string> Solve_2_Distinct()
    {
        for (int i = 13; i < _input.Length; ++i)
        {
            var set = new[] {
                _input[i - 13], _input[i - 12], _input[i - 11], _input[i - 10], _input[i - 9], _input[i - 8],_input[i - 7],
                _input[i - 6], _input[i - 5], _input[i - 4], _input[i - 3], _input[i - 2], _input[i - 1], _input[i]
            };

            if (set.Distinct().Count() == 14)
            {
                return new($"{i + 1}");
            }
        }

        throw new SolvingException();
    }

    public ValueTask<string> Solve_1_WithSpan() => new($"{SolveWithSpan(_input, 4)}");

    public ValueTask<string> Solve_2_WithSpan() => new($"{SolveWithSpan(_input, 14)}");

    public ValueTask<string> Solve_1_WithQueue() => new($"{SolveWithQueue(_input, 4)}");

    public ValueTask<string> Solve_2_WithQueue() => new($"{SolveWithQueue(_input, 14)}");

    private static int SolveWithEnumerableRange(string input, int window)
    {
        for (int i = window - 1; i < input.Length; ++i)
        {
            var set = new HashSet<char>(Enumerable.Range(i - window + 1, window).Select(n => input[n]));
            if (set.Count == window)
            {
                return i + 1;
            }
        }

        throw new SolvingException();
    }

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

    public static int SolveWithQueue(string input, int window)
    {
        var queue = new Queue<char>(input[0..window]);
        for (int i = window; i < input.Length; ++i)
        {
            if (queue.Distinct().Count() == window)
            {
                return i;
            }

            queue.Dequeue();
            queue.Enqueue(input[i]);
        }

        throw new SolvingException();
    }
}
