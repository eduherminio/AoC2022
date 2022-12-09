/*
 *  |                    Method |     Mean |     Error |    StdDev | Ratio | RatioSD |     Gen0 |    Gen1 |    Gen2 | Allocated | Alloc Ratio |
 *  |-------------------------- |---------:|----------:|----------:|------:|--------:|---------:|--------:|--------:|----------:|------------:|
 *  |            Part1_Distance | 2.008 ms | 0.0385 ms | 0.0412 ms |  1.00 |    0.00 | 144.5313 | 66.4063 | 46.8750 | 847.13 KB |        1.00 |
 *  |            Part1_Original | 2.515 ms | 0.0377 ms | 0.0353 ms |  1.25 |    0.03 | 144.5313 | 66.4063 | 46.8750 | 847.06 KB |        1.00 |
 *  | Part1 (ChebyshevDistance) | 1.974 ms | 0.0386 ms | 0.0502 ms |  0.98 |    0.03 | 144.5313 | 66.4063 | 46.8750 | 847.13 KB |        1.00 |
 *  | Solve_1_ManhattanDistance | 1.994 ms | 0.0371 ms | 0.0347 ms |  0.99 |    0.03 | 144.5313 | 66.4063 | 46.8750 | 847.13 KB |        1.00 |
 *
 *  |                    Method |     Mean |     Error |    StdDev | Ratio | RatioSD |     Gen0 |     Gen1 | Allocated | Alloc Ratio |
 *  |-------------------------- |---------:|----------:|----------:|------:|--------:|---------:|---------:|----------:|------------:|
 *  |            Part2_Distance | 3.076 ms | 0.0560 ms | 0.0496 ms |  1.00 |    0.00 | 753.9063 | 148.4375 |   1.87 MB |        1.00 |
 *  |            Part2_Original | 6.954 ms | 0.0991 ms | 0.0879 ms |  2.26 |    0.03 | 742.1875 | 148.4375 |   1.87 MB |        1.00 |
 *  | Part2 (ChebyshevDistance) | 2.836 ms | 0.0538 ms | 0.0576 ms |  0.92 |    0.03 | 753.9063 | 148.4375 |   1.87 MB |        1.00 |
 *  | Solve_2_ManhattanDistance | 3.062 ms | 0.0576 ms | 0.0510 ms |  1.00 |    0.02 | 757.8125 | 148.4375 |   1.87 MB |        1.00 |
 */

using BenchmarkDotNet.Attributes;

namespace AoC_2022.Benchmarks;

public class Day_09_Part1 : BaseDayBenchmark
{
    private readonly Day_09 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part1() => await _problem.Solve_1();

    [Benchmark]
    public async Task<string> Part1_Original() => await _problem.Solve_1_Original();

    [Benchmark]
    public async Task<string> Solve_1_ManhattanDistance() => await _problem.Solve_1_ManhattanDistance();

    [Benchmark]
    public async Task<string> Solve_1_Distance() => await _problem.Solve_1_Distance();
}

public class Day_09_Part2 : BaseDayBenchmark
{
    private readonly Day_09 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part2() => await _problem.Solve_2();

    [Benchmark]
    public async Task<string> Part2_Original() => await _problem.Solve_2_Original();

    [Benchmark]
    public async Task<string> Solve_2_ManhattanDistance() => await _problem.Solve_2_ManhattanDistance();


    [Benchmark]
    public async Task<string> Solve_2_Distance() => await _problem.Solve_2_Distance();
}
