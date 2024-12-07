using Utils;

namespace Year2024.Day02;

// https://adventofcode.com/2024/day/2

public class Day02 : BaseDay
{
    protected override Answers Part1Answers { get; } = new(2, 421);
    protected override Answers Part2Answers { get; } = new(4, 476);

    protected override dynamic SolvePart1(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var validReports = 0;

        foreach (var line in lines)
        {
            var values = line.SplitToIntegers();

            if (AreValuesValid(values))
            {
                validReports++;
            }
        }

        return validReports;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var validReports = 0;

        foreach (var line in lines)
        {
            var values = line.SplitToIntegers();

            if (AreValuesValid(values) || DampenerCanBeApplied(values))
            {
                validReports++;
            }
        }

        return validReports;
    }

    private bool AreValuesValid(int[] values)
    {
        if (values == null || values.Length < 2)
            return false;

        int diff = values[1] - values[0];
        if (Math.Abs(diff) > 3 || diff == 0)
            return false;

        bool increasing = diff > 0;

        for (int i = 2; i < values.Length; i++)
        {
            diff = values[i] - values[i - 1];
            if (Math.Abs(diff) > 3 || diff == 0 || (diff > 0) != increasing)
                return false;
        }

        return true;
    }

    private bool DampenerCanBeApplied(int[] values)
    {
        int count = values.Length;

        for (int i = 0; i < count; i++)
        {
            // Create a copy excluding the current element
            int[] valuesCopy = new int[count - 1];
            for (int j = 0, k = 0; j < count; j++)
            {
                if (j != i)
                {
                    valuesCopy[k++] = values[j];
                }
            }

            if (AreValuesValid(valuesCopy))
                return true;
        }

        return false;
    }
}