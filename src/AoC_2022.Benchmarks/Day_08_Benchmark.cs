/*
 *  |              Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |      Gen0 |     Gen1 | Allocated | Alloc Ratio |
 *  |-------------------- |---------:|----------:|----------:|---------:|------:|--------:|----------:|---------:|----------:|------------:|
 *  |               Part1 | 5.682 ms | 0.1125 ms | 0.1539 ms | 5.621 ms |  1.00 |    0.00 | 3210.9375 |        - |   6.42 MB |        1.00 |
 *  | Part1_ReusingRanges | 8.519 ms | 0.7437 ms | 2.1930 ms | 7.006 ms |  2.00 |    0.22 | 2929.6875 | 132.8125 |    5.9 MB |        0.92 |
 *
 9  |         Method |      Mean |     Error |    StdDev | Ratio | RatioSD |       Gen0 |     Gen1 | Allocated | Alloc Ratio |
 9  |--------------- |----------:|----------:|----------:|------:|--------:|-----------:|---------:|----------:|------------:|
 9  |          Part2 |  9.654 ms | 0.1658 ms | 0.1470 ms |  1.00 |    0.00 |  3578.1250 | 312.5000 |   7.31 MB |        1.00 |
 9  | Part2_Original | 19.657 ms | 0.3740 ms | 0.4002 ms |  2.04 |    0.04 | 13343.7500 |        - |  26.65 MB |        3.64 |
 */

using BenchmarkDotNet.Attributes;

namespace AoC_2022.Benchmarks;

public class Day_08_Part1 : BaseDayBenchmark
{
    private readonly Day_08 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part1() => await _problem.Solve_1();

    [Benchmark]
    public async Task<string> Part1_ReusingRanges() => await _problem.Solve_1_ReusingRanges();
}

public class Day_08_Part2 : BaseDayBenchmark
{
    private readonly Day_08 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part2() => await _problem.Solve_2();

    [Benchmark]
    public async Task<string> Part2_Original() => await _problem.Solve_2_Original();
}
