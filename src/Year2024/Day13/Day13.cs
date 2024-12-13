using Utils;
using Utils.Extensions;

namespace Year2024.Day13;

public class Day13 : BaseDay
{
    protected override Answers Part1Answers => new(480, 27105);
    protected override Answers Part2Answers => new(875318608908, 101726882250942);
    protected override dynamic SolvePart1(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var result = Solve(lines);

        return result;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var result = Solve(lines, true);

        return result;
    }

    private static long Solve(string[] lines, bool adjustPoints = false)
    {
        var result = 0L;

        for (var i = 0; i < lines.Length;)
        {
            // Get buttonA
            var s = lines[i].Split(",").Select(x => x.Split("+")[^1].Trim()).ToArray();
            var xA = s[0].ToLong();
            var yA = s[1].ToLong();
            i++;

            // Get buttonB
            s = lines[i].Split(",").Select(x => x.Split("+")[^1].Trim()).ToArray();
            var xB = s[0].ToLong();
            var yB = s[1].ToLong();
            i++;

            // Get Goal
            s = lines[i].Split(",").Select(x => x.Split("=")[^1].Trim()).ToArray();
            var xP = s[0].ToLong();
            var yP = s[1].ToLong();
            i += 2;

            if (adjustPoints)
            {
                xP += 10000000000000;
                yP += 10000000000000;
            }

            var B = (yP * xA - xP * yA) / (yB * xA - xB * yA);
            var A = (xP - B * xB) / xA;

            if (A >= 0 && B >= 0 && B * xB + A * xA == xP && B * yB + A * yA == yP)
            {
                result += A * 3 + B;
            }
        }

        return result;
    }
}