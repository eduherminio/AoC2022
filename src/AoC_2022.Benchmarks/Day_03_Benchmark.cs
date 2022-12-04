using BenchmarkDotNet.Attributes;

namespace AoC_2022.Benchmarks;

public class Day_03_Part1 : BaseDayBenchmark
{
    private readonly Day_03 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part1() => await _problem.Solve_1();

    [Benchmark]
    public async Task<string> WithoutSpan() => await _problem.Solve_1_WithoutSpan();
}

public class Day_03_Part2 : BaseDayBenchmark
{
    private readonly Day_03 _problem = new();

    [Benchmark(Baseline = true)]
    public async Task<string> Part2() => await _problem.Solve_2();

    [Benchmark]
    public async Task<string> WithSpan() => await _problem.Solve_2_WithSpan();

    [Benchmark]
    public async Task<string> WithLinq() => await _problem.Solve_2_Linq();
}
