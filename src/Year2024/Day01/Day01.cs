using Utils;

namespace Year2024.Day01;

public class Day01 : BaseDay
{
    protected override string Day { get; } = "Day01";
    protected override Answers Part1Answers { get; } = new(0, 0);
    protected override Answers Part2Answers { get; } = new(0, 0);

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
        return 0;
    }

    private int SolvePart2(string inputFile)
    {
        return 0;
    }
}