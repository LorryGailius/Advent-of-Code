﻿using Utils;

namespace Year2024.Day01;

// https://adventofcode.com/2024/day/1

public class Day01 : BaseDay
{
    protected override Answers Part1Answers { get; } = new(11, 1189304);
    protected override Answers Part2Answers { get; } = new(31, 24349736);

    protected override dynamic SolvePart1(string inputFile)
    {
        var lists = Input.ToLists<int>(inputFile);

        var list1 = lists[0];
        var list2 = lists[1];

        list1.Sort();
        list2.Sort();

        var diff = list1.Zip(list2).Sum(x => Math.Abs(x.First - x.Second));

        return diff;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var lists = Input.ToLists<int>(inputFile);

        var list1 = lists[0];
        var list2 = lists[1];

        list1.Sort();
        list2.Sort();

        var similarityScore = list1.Sum(x =>
        {
            return list2.Count(y => y == x) * x;
        });

        return similarityScore;
    }
}