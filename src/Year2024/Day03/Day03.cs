using System.Text.RegularExpressions;
using Utils;

namespace Year2024.Day03;

// https://adventofcode.com/2024/day/2

public class Day03 : BaseDay
{
    protected override string Day { get; } = "Day03";
    protected override Answers Part1Answers { get; } = new(161, 174336360);
    protected override Answers Part2Answers { get; } = new(4, 476);

    protected override int Solve(string inputFile, int part)
    {
        return part switch
        {
            1 => SolvePart1(inputFile),
            2 => SolvePart2(inputFile),
            _ => 0
        };
    }

    private int SolvePart1(string inputFile)
    {
        var regex = @"mul\((\d+),(\d+)\)";
        var text = Input.ToString(inputFile);
        var matches = Regex.Matches(text, regex).ToList();

        var result = 0;

        foreach (var match in matches)
        {
            var value1 = match.Groups[1].Value.ToInt();
            var value2 = match.Groups[2].Value.ToInt();

            result += value1 * value2;
        }

        return result;
    }

    private int SolvePart2(string inputFile)
    {
        return 0;
    }
}