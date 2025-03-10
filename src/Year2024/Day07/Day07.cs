﻿using Utils;
using Utils.Extensions;

namespace Year2024.Day07;

// https://adventofcode.com/2024/day/7

public class Day07 : BaseDay
{
    protected override Answers Part1Answers => new(3749, 3351424677624);
    protected override Answers Part2Answers => new(11387, 204976636995111);
    protected override dynamic SolvePart1(string inputFile)
    {
        var tests = GetValues(inputFile);

        long result = 0;

        foreach (var test in tests)
        {
            if (IsValid(test))
            {
                result += test.Target;
            }
        }

        return result;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var tests = GetValues(inputFile);

        long result = 0;

        foreach (var test in tests)
        {
            if (IsValid(test, true))
            {
                result += test.Target;
            }
        }

        return result;
    }

    private static bool IsValid(Record record, bool concatEnabled = false)
    {
        if (record.Values.Count == 0)
        {
            return false;
        }

        //return CanFormResult(record.Target, record.Values[1..], record.Values[0], concatEnabled);
        return CanFormResultReverse(record.Values, record.Target, concatEnabled);
    }

    private static bool CanFormResult(long target, List<long> nums, long current, bool concatEnabled = false)
    {
        if (!nums.Any())
        {
            return target == current;
        }

        return CanFormResult(target, nums[1..], current + nums[0], concatEnabled) || 
               CanFormResult(target, nums[1..], current * nums[0], concatEnabled) ||
               (concatEnabled && CanFormResult(target, nums[1..], long.Parse($"{current}{nums[0]}"), concatEnabled));
    }

    private static bool CanFormResultReverse(List<long> nums, long current, bool concatEnabled = false)
    {
        if(nums.Count == 1)
        {
            return current == nums[0];
        }

        var last = nums[^1];

        if (current > nums[^1] && CanFormResultReverse(nums[..^1], current - nums[^1], concatEnabled))
        {
            return true;
        }

        if (current % nums[^1] == 0 && CanFormResultReverse(nums[..^1], current / nums[^1], concatEnabled))
        {
            return true;
        }

        if (concatEnabled)
        {
            var divideBy = (long)Math.Pow(10, Math.Floor(Math.Log10(nums[^1])) + 1);
            if (current % divideBy == nums[^1] && CanFormResultReverse(nums[..^1], current / divideBy, concatEnabled))
            {
                return true;
            }
        }

        return false;
    }

    private static List<Record> GetValues(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var result = new List<Record>();

        foreach (var line in lines)
        {
            var parts = line.Split(":");
            var key = long.Parse(parts[0]);
            var values = parts[1].SplitToLongs().ToList();

            result.Add(new Record(key, values));
        }

        return result;
    }

    internal record Record(
        long Target,
        List<long> Values);
}