using MoreLinq;
using SheepTools.Model;
using System;
using System.Runtime.InteropServices;

namespace AoC_2022;

public class Day_13 : BaseDay
{
    private readonly List<(string, string)> _input;

    public Day_13()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        Node currentNode = new Node("root");

        //_input.ForEach((pair, pairIndex) =>
        //{
        //    pair.Item1.ForEach((ch, chIndex) =>
        //    {
        //        if (ch == '[')
        //        {
        //            currentNode.Children.Add(new(""));
        //            currentNode = currentNode.Children.Last();
        //        }
        //        else if (ch == ']')
        //        {

        //        }
        //        else
        //        {
        //            var n = int.Parse(pair.Item1.Skip(chIndex).TakeWhile(ch => ch == ',' || ch == '[' || ch == ']'));
        //        }
        //    })
        //});

        static List<int> ExtractList(string str)
        {
            return str.Split(',').Select(int.Parse).ToList();
        }

        List<int> correctPairs = new(_input.Count);

        _input.ForEach((pair, index) =>
        {
            var l1 = pair.Item1
                .Replace("[]", "-1000000")
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            var l2 = pair.Item2
                .Replace("[]", "-1000000")
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            bool isValid = l1.Count == l2.Count
                ? pair.Item1.Length <= pair.Item2.Length
                : l1.Count <= l2.Count;

            for (int i = 0; i < l1.Count; ++i)
            {
                if (i < l2.Count)
                {
                    if (l1[i] < l2[i])
                    {
                        isValid = true;
                        break;
                    }
                    else if (l1[i] > l2[i])
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            if (isValid)
            {
                correctPairs.Add(index + 1);
            }

            //var line1 = string.Join(',', line1Original.Split(',').Select(item => item.Length >= 3 && item[0] == '[' && item[^1] == ']' ? item : $"[{item}]"));
            //var line2 = string.Join(',', line2Original.Split(',').Select(item => item.Length >= 3 && item[0] == '[' && item[^1] == ']' ? item : $"[{item}]"));



            //for (int i = 0; i < line1.Length; ++i)
            //{
            //    var ch1 = line1[i];
            //    var ch2 = line1[i];

            //    if (ch1 == '[')
            //    {
            //        var n1 = int.Parse(line1[i..line1[i..].IndexOf(']')].TrimStart('['));
            //        var n2 = int.Parse(line2[i..line2[i..].IndexOf(']')].TrimStart('['));

            //        if (n1 < n2)
            //        {
            //            correctPairs.Add(i + 1);
            //        }
            //    }
            //}

            //Console.WriteLine(line1);
            //Console.WriteLine(line2);
            //Console.WriteLine();
        });

        return new($"{correctPairs.Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
    }

    private List<(string, string)> ParseInput()
    {
        return ParsedFile.ReadAllGroupsOfLines(InputFilePath).Select(g => (g[0], g[1])).ToList();
    }
}
