using AoCHelper;
using NUnit.Framework;

namespace AoC_2022.Test;

#pragma warning disable IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
public class SolutionTests
{
    //[TestCase(typeof(Day_01), "1527", "1575")]
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
