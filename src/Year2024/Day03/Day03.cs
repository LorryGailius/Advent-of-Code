using System.Text;
using System.Text.RegularExpressions;
using Utils;
using Utils.Extensions;

namespace Year2024.Day03;

// https://adventofcode.com/2024/day/3

public class Day03 : BaseDay
{
    protected override Answers Part1Answers { get; } = new(161, 174336360);
    protected override Answers Part2Answers { get; } = new(48, 88802350);

    protected override dynamic SolvePart1(string inputFile)
    {
        return CalculateMulOperations(Input.ToString(inputFile));
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var input = Input.ToString(inputFile);

        return CalculateMulOperations(GetConditionalInput(input));
    }

    public int CalculateMulOperations(string input)
    {
        return Regex.Matches(input, @"mul\((\d+),(\d+)\)")
            .Sum(x => x.Groups[1].Value.ToInt() * x.Groups[2].Value.ToInt());
    }

    public string GetConditionalInput(string input)
    {
        const string dontPattern = "don't()";
        const string doPattern = "do()";

        var outputBuffer = new StringBuilder();

        var read = true;

        for (var i = 0; i < input.Length; i++)
        {
            if (i + 6 < input.Length && input.Substring(i, 7) == dontPattern)
            {
                read = false;
                i += 6;
                continue;
            }

            if (i + 3 < input.Length && input.Substring(i, 4) == doPattern)
            {
                read = true;
                i += 3;
                continue;
            }

            if (read)
            {
                outputBuffer.Append(input[i]);
            }
        }

        return outputBuffer.ToString();
    }
}