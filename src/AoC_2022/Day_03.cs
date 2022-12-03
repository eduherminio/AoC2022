﻿namespace AoC_2022;

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

            result += CalculatePriority(firstHalf[index]);
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        for (int i = 0; i < _input.Length - 2; i += 3)
        {
            var first = _input[i];
            var second = _input[i + 1];
            var third = _input[i + 2];

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

            result += CalculatePriority(first[index]);
        }

        return new($"{result}");
    }

    /// <summary>
    /// Significantly efficient
    /// |      Method |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 | Allocated | Alloc Ratio |
    /// |------------ |---------:|---------:|---------:|------:|--------:|--------:|----------:|------------:|
    /// |       Part1 | 15.95 us | 0.240 us | 0.200 us |  1.00 |    0.00 |  0.0305 |     104 B |        1.00 |
    /// | WithoutSpan | 25.76 us | 0.491 us | 0.546 us |  1.62 |    0.04 | 16.3879 |   34312 B |      329.92 |
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> Solve_1_WithoutSpan()
    {
        int result = 0;

        foreach (var rucksack in _input)
        {
            var firstHalf = rucksack[..(rucksack.Length / 2)];
            var secondHalf = rucksack[(rucksack.Length / 2)..];

            int index = 0;
            while (!secondHalf.Contains(firstHalf[index]))
            {
                index++;
            }

            result += CalculatePriority(firstHalf[index]);
        }

        return new($"{result}");
    }

    /// <summary>
    /// Slightly less efficient
    /// |      Method |     Mean |    Error |   StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
    /// |------------ |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
    /// |    WithSpan | 13.87 us | 0.135 us | 0.126 us |  1.00 |    0.00 | 0.0458 |     104 B |        1.00 |
    /// |       Part2 | 12.45 us | 0.199 us | 0.204 us |  0.90 |    0.02 | 0.0458 |     104 B |        1.00 |
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> Solve_2_WithSpan()
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

            result += CalculatePriority(first[index]);
        }

        return new($"{result}");
    }

    private static int CalculatePriority(char doubleElement)
    {
        return char.IsUpper(doubleElement)
            ? doubleElement - 'A' + 27
            : doubleElement - 'a' + 1;
    }

    private string[] ParseInput() => File.ReadAllLines(InputFilePath);
}
