﻿namespace AoC_2022;

public class Day_08 : BaseDay
{
    private readonly List<List<int>> _input;

    public Day_08()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var maxX = _input[0].Count;
        var maxY = _input.Count;

        int visible = (2 * maxX) + (2 * maxY) - 4;

        for (int y = 1; y < maxY - 1; ++y)
        {
            for (int x = 1; x < maxX - 1; ++x)
            {
                var point = _input[y][x];

                var rangeLeft = Enumerable.Range(0, x);
                var rangeUp = Enumerable.Range(0, y);
                var rangeRight = Enumerable.Range(x + 1, maxX - x - 1);
                var rangeDown = Enumerable.Range(y + 1, maxY - y - 1);

                if (rangeLeft.Select(customX => _input[y][customX]).All(value => value < point)
                    || rangeRight.Select(customX => _input[y][customX]).All(value => value < point)
                    || rangeUp.Select(customY => _input[customY][x]).All(value => value < point)
                    || rangeDown.Select(customY => _input[customY][x]).All(value => value < point))
                {
                    ++visible;
                }
            }
        }

        return new($"{visible}");
    }

    public override ValueTask<string> Solve_2()
    {
        var maxX = _input[0].Count;
        var maxY = _input.Count;

        int visible = int.MinValue;

        var leftRanges = new Dictionary<int, List<int>>(maxX);
        var rightRanges = new Dictionary<int, List<int>>(maxX);
        var upRanges = new Dictionary<int, List<int>>(maxY);
        var downRanges = new Dictionary<int, List<int>>(maxY);

        for (int y = 1; y < maxY - 1; ++y)
        {
            for (int x = 1; x < maxX - 1; ++x)
            {
                var point = _input[y][x];

                if (!leftRanges.TryGetValue(x, out var rangeLeft))
                {
                    rangeLeft = leftRanges[x] = Enumerable.Range(0, x).Reverse().ToList();
                }

                var left = rangeLeft
                    .TakeWhile((customX, index) =>
                        (_input[y][customX] < point
                            && (index == 0 || _input[y][rangeLeft[index - 1]] < point))
                        || index == 0
                        || _input[y][rangeLeft[index - 1]] < point)
                    .Count();

                if (left == 0) continue;

                if (!rightRanges.TryGetValue(x, out var rangeRight))
                {
                    rangeRight = rightRanges[x] = Enumerable.Range(x + 1, maxX - x - 1).ToList();
                }

                var right = rangeRight
                    .TakeWhile((customX, index) =>
                        (_input[y][customX] < point
                            && (index == 0 || _input[y][rangeRight[index - 1]] < point))
                        || index == 0
                        || _input[y][rangeRight[index - 1]] < point)
                    .Count();

                if (right == 0) continue;

                if (!upRanges.TryGetValue(y, out var rangeUp))
                {
                    rangeUp = upRanges[y] = Enumerable.Range(0, y).Reverse().ToList();
                }

                var up = rangeUp
                    .TakeWhile((customY, index) =>
                        (_input[customY][x] < point
                            && (index == 0 || _input[rangeUp[index - 1]][x] < point))
                        || index == 0
                        || _input[rangeUp[index - 1]][x] < point)
                    .Count();

                if (up == 0) continue;

                if (!downRanges.TryGetValue(y, out var rangeDown))
                {
                    rangeDown = downRanges[y] = Enumerable.Range(y + 1, maxY - y - 1).ToList();
                }

                var down = rangeDown
                    .TakeWhile((customY, index) =>
                        (_input[customY][x] < point
                            && (index == 0 || _input[rangeDown[index - 1]][x] < point))
                        || index == 0
                        || _input[rangeDown[index - 1]][x] < point)
                    .Count();

                var product = left * right * up * down;

                if (product > visible)
                {
                    visible = product;
                }
            }
        }

        return new($"{visible}");
    }

    public ValueTask<string> Solve_1_ReusingRanges()
    {
        var maxX = _input[0].Count;
        var maxY = _input.Count;

        int visible = (2 * maxX) + (2 * maxY) - 4;

        var leftRanges = new Dictionary<int, List<int>>(maxX);
        var rightRanges = new Dictionary<int, List<int>>(maxX);
        var upRanges = new Dictionary<int, List<int>>(maxY);
        var downRanges = new Dictionary<int, List<int>>(maxY);

        for (int y = 1; y < maxY - 1; ++y)
        {
            for (int x = 1; x < maxX - 1; ++x)
            {
                var point = _input[y][x];

                if (!leftRanges.TryGetValue(x, out var rangeLeft))
                {
                    rangeLeft = leftRanges[x] = Enumerable.Range(0, x).ToList();
                }

                if (!upRanges.TryGetValue(y, out var rangeUp))
                {
                    rangeUp = upRanges[y] = Enumerable.Range(0, y).ToList();
                }

                if (!rightRanges.TryGetValue(x, out var rangeRight))
                {
                    rangeRight = rightRanges[x] = Enumerable.Range(x + 1, maxX - x - 1).ToList();
                }

                if (!downRanges.TryGetValue(y, out var rangeDown))
                {
                    rangeDown = downRanges[y] = Enumerable.Range(y + 1, maxY - y - 1).ToList();
                }

                if (rangeLeft.Select(customX => _input[y][customX]).All(value => value < point)
                    || rangeRight.Select(customX => _input[y][customX]).All(value => value < point)
                    || rangeUp.Select(customY => _input[customY][x]).All(value => value < point)
                    || rangeDown.Select(customY => _input[customY][x]).All(value => value < point))
                {
                    ++visible;
                }
            }
        }

        return new($"{visible}");
    }

    public ValueTask<string> Solve_2_Original()
    {
        var maxX = _input[0].Count;
        var maxY = _input.Count;

        int visible = int.MinValue;

        for (int y = 0; y < maxY; ++y)
        {
            for (int x = 0; x < maxX; ++x)
            {
                var point = _input[y][x];

                var rangeLeft = Enumerable.Range(0, x).Reverse();
                var rangeUp = Enumerable.Range(0, y).Reverse();
                var rangeRight = Enumerable.Range(x + 1, maxX - x - 1);
                var rangeDown = Enumerable.Range(y + 1, maxY - y - 1);

                var left = rangeLeft
                    .TakeWhile((customX, index) =>
                        (_input[y][customX] < point
                            && (index == 0 || _input[y][rangeLeft.ElementAt(index - 1)] < point))
                        || index == 0
                        || _input[y][rangeLeft.ElementAt(index - 1)] < point)
                    .Count();

                var right = rangeRight
                    .TakeWhile((customX, index) =>
                        (_input[y][customX] < point
                            && (index == 0 || _input[y][rangeRight.ElementAt(index - 1)] < point))
                        || index == 0
                        || _input[y][rangeRight.ElementAt(index - 1)] < point)
                    .Count();

                var up = rangeUp
                    .TakeWhile((customY, index) =>
                        (_input[customY][x] < point
                            && (index == 0 || _input[rangeUp.ElementAt(index - 1)][x] < point))
                        || index == 0
                        || _input[rangeUp.ElementAt(index - 1)][x] < point)
                    .Count();

                var down = rangeDown
                    .TakeWhile((customY, index) =>
                        (_input[customY][x] < point
                            && (index == 0 || _input[rangeDown.ElementAt(index - 1)][x] < point))
                        || index == 0
                        || _input[rangeDown.ElementAt(index - 1)][x] < point)
                    .Count();

                var product = left * right * up * down;

                if (product > visible)
                {
                    visible = product;
                }
            }
        }

        return new($"{visible}");
    }

    private List<List<int>> ParseInput()
    {
        var result = new List<List<int>>();
        foreach (var inputLine in File.ReadLines(InputFilePath))
        {
            var line = new List<int>();
            foreach (var ch in inputLine)
            {
                line.Add(ch - '0');
            }
            result.Add(line);
        }

        return result;
    }
}
