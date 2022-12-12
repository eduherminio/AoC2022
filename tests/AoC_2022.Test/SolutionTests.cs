using AoCHelper;
using NUnit.Framework;

namespace AoC_2022.Test;

#pragma warning disable IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
public static class SolutionTests
{
    [TestCase(typeof(Day_01), "66186", "196804")]
    [TestCase(typeof(Day_02), "14163", "12091")]
    [TestCase(typeof(Day_03), "8039", "2510")]
    [TestCase(typeof(Day_04), "471", "888")]
    [TestCase(typeof(Day_05), "WSFTMRHPP", "GSLCMFBRP")]
    [TestCase(typeof(Day_06), "1920", "2334")]
    [TestCase(typeof(Day_07), "1447046", "578710")]
    [TestCase(typeof(Day_08), "1829", "291840")]
    [TestCase(typeof(Day_09), "6406", "2643")]
    [TestCase(typeof(Day_10), "10760", @"
####.###...##..###..#..#.####..##..#..#.
#....#..#.#..#.#..#.#..#.#....#..#.#..#.
###..#..#.#....#..#.####.###..#....####.
#....###..#.##.###..#..#.#....#.##.#..#.
#....#....#..#.#....#..#.#....#..#.#..#.
#....#.....###.#....#..#.#.....###.#..#.")]
    [TestCase(typeof(Day_11), "62491", "17408399184")]
    [TestCase(typeof(Day_12), "484", "478")]
    public static async Task Test(Type type, string sol1, string sol2)
    {
        if (Activator.CreateInstance(type) is BaseProblem instance)
        {
            Assert.AreEqual(sol1, await instance.Solve_1());
            Assert.AreEqual(sol2, await instance.Solve_2());
        }
        else
        {
            Assert.Fail($"{type} is not a BaseDay");
        }
    }
}
#pragma warning restore IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
