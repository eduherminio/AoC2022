/*
*   |          Method |     Mean |    Error |    StdDev |   Median | Ratio | RatioSD |     Gen0 | Allocated | Alloc Ratio |
*   |---------------- |---------:|---------:|----------:|---------:|------:|--------:|---------:|----------:|------------:|
*   |           Part1 | 188.4 us |  3.73 us |   7.01 us | 189.3 us |  1.00 |    0.00 | 154.0527 | 314.77 KB |        1.00 |
*   |  Part1_Distinct | 538.9 us | 39.62 us | 114.95 us | 489.5 us |  3.01 |    0.70 | 330.0781 | 674.05 KB |        2.14 |
*   |  Part1_WithSpan | 474.2 us |  9.40 us |  26.81 us | 474.3 us |  2.51 |    0.17 | 330.0781 | 674.05 KB |        2.14 |
*   | Part1_WithQueue | 515.4 us |  8.72 us |   9.33 us | 518.0 us |  2.74 |    0.13 | 249.0234 |  509.6 KB |        1.62 |
*
*   |          Method |     Mean |     Error |    StdDev | Ratio | RatioSD |     Gen0 | Allocated | Alloc Ratio |
*   |---------------- |---------:|----------:|----------:|------:|--------:|---------:|----------:|------------:|
*   |           Part2 | 1.042 ms | 0.0192 ms | 0.0310 ms |  1.00 |    0.00 | 521.4844 |   1.04 MB |        1.00 |
*   |  Part2_Distinct | 1.164 ms | 0.0232 ms | 0.0485 ms |  1.12 |    0.06 | 603.5156 |   1.21 MB |        1.16 |
*   |  Part2_WithSpan | 1.150 ms | 0.0228 ms | 0.0515 ms |  1.11 |    0.05 | 603.5156 |   1.21 MB |        1.16 |
*   | Part2_WithQueue | 1.736 ms | 0.0343 ms | 0.0592 ms |  1.67 |    0.09 | 636.7188 |   1.27 MB |        1.22 |
 */

using BenchmarkDotNet.Attributes;

namespace AoC_2022.Benchmarks;

public class Day_06_Part1 : BaseDayBenchmark
{
    private readonly Day_06 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part1() => await _problem.Solve_1();

    [Benchmark]
    public async Task<string> Part1_EnumerableRange() => await _problem.Solve_1_WithEnumerableRange();

    [Benchmark]
    public async Task<string> Part1_Distinct() => await _problem.Solve_1_Distinct();

    [Benchmark]
    public async Task<string> Part1_WithSpan() => await _problem.Solve_1_WithSpan();

    [Benchmark]
    public async Task<string> Part1_WithQueue() => await _problem.Solve_1_WithQueue();
}

public class Day_06_Part2 : BaseDayBenchmark
{
    private readonly Day_06 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part2() => await _problem.Solve_2();

    [Benchmark]
    public async Task<string> Part2_EnumerableRange() => await _problem.Solve_2_WithEnumerableRange();

    [Benchmark]
    public async Task<string> Part2_Distinct() => await _problem.Solve_2_Distinct();

    [Benchmark]
    public async Task<string> Part2_WithSpan() => await _problem.Solve_2_WithSpan();

    [Benchmark]
    public async Task<string> Part2_WithQueue() => await _problem.Solve_2_WithQueue();
}
