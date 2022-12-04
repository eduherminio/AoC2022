namespace AoC_2022;

public class Day_04 : BaseDay
{
    private readonly List<((int Start, int End) A, (int Start, int End) B)> _input;

    public Day_04()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;
        foreach (var (A, B) in _input)
        {
            if (A.Start == B.Start)
            {
                ++result;
            }
            else if (A.Start < B.Start)     // A..B..
            {
                if (A.End >= B.End)         // A..B..B..A
                {
                    ++result;
                }
            }
            else                            // B..A..
            {
                if (A.End <= B.End)         // B..A..A..B
                {
                    ++result;
                }
            }
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;
        foreach (var (A, B) in _input)
        {
            if (A.Start == B.Start || A.End == B.End)
            {
                ++result;
            }
            else if (A.Start < B.Start && A.End >= B.Start)     // A..B..A
            {
                ++result;
            }
            else if (A.Start > B.Start && B.End >= A.Start)     // B..A..B
            {
                ++result;
            }
        }

        return new($"{result}");
    }

    private IEnumerable<((int, int), (int, int))> ParseInput()
    {
        var file = new ParsedFile(InputFilePath, existingSeparator: new char[] { ',', '-' });
        while (!file.Empty)
        {
            var line = file.NextLine();
            yield return ((line.NextElement<int>(), line.NextElement<int>()), (line.NextElement<int>(), line.NextElement<int>()));
        }
    }
}
