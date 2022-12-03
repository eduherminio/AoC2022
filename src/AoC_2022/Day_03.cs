namespace AoC_2022;

public class Day_03 : BaseDay
{
    private readonly string[] _input;

    public Day_03()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        foreach (var rucksack in _input)
        {
            var span = rucksack.AsSpan();
            var firstHalf = span[..(rucksack.Length / 2)];
            var secondHalf = span.Slice(rucksack.Length / 2, rucksack.Length / 2);

            int index = 0;
            while (!secondHalf.Contains(firstHalf[index]))
            {
                index++;
            }

            var doubleElement = firstHalf[index];
            result += char.IsUpper(doubleElement)
                ? doubleElement - 'A' + 27
                : doubleElement - 'a' + 1;
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        for (int i = 0; i < _input.Length - 2; i += 3)
        {
            var first = _input[i].AsSpan();
            var second = _input[i + 1].AsSpan();
            var third = _input[i + 2].AsSpan();

            int index = 0;
            while (true)
            {
                var item = first[index];
                if (second.Contains(item) && third.Contains(item))
                {
                    break;
                }
                index++;
            }

            var doubleElement = first[index];
            result += char.IsUpper(doubleElement)
                ? doubleElement - 'A' + 27
                : doubleElement - 'a' + 1;
        }

        return new($"{result}");
    }

    private string[] ParseInput() => File.ReadAllLines(InputFilePath);
}
