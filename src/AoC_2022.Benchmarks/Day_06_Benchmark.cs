/*
*   |         Method |      Mean |    Error |   StdDev | Ratio | RatioSD |     Gen0 | Allocated | Alloc Ratio |
*   |--------------- |----------:|---------:|---------:|------:|--------:|---------:|----------:|------------:|
*   |          Part1 |  77.19 us | 1.522 us | 2.895 us |  1.00 |    0.00 |  51.2695 | 314.77 KB |        1.00 |
*   | Part1_WithSpan | 175.66 us | 3.494 us | 8.027 us |  2.29 |    0.14 | 109.8633 | 674.05 KB |        2.14 |
*
*   |         Method |     Mean |    Error |   StdDev | Ratio | RatioSD |     Gen0 | Allocated | Alloc Ratio |
*   |--------------- |---------:|---------:|---------:|------:|--------:|---------:|----------:|------------:|
*   |          Part2 | 508.3 us | 10.09 us | 15.70 us |  1.00 |    0.00 | 173.8281 |   1.04 MB |        1.00 |
*   | Part2_WithSpan | 523.3 us | 10.25 us | 15.02 us |  1.03 |    0.03 | 201.1719 |   1.21 MB |        1.16 |
 */

using BenchmarkDotNet.Attributes;

namespace AoC_2022.Benchmarks;

public class Day_06_Part1 : BaseDayBenchmark
{
    private readonly Day_06 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part1() => await _problem.Solve_1();

    [Benchmark]
    public async Task<string> Part1_WithSpan() => await _problem.Solve_1_WithSpan();
}

public class Day_06_Part2 : BaseDayBenchmark
{
    private readonly Day_06 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part2() => await _problem.Solve_2();

    [Benchmark]
    public async Task<string> Part2_WithSpan() => await _problem.Solve_2_WithSpan();
}
