using Xunit;

namespace Utils;

public abstract class BaseDay
{
    protected abstract Answers Part1Answers { get; }
    protected abstract Answers Part2Answers { get; }
    protected abstract dynamic SolvePart1(string inputFile);
    protected abstract dynamic SolvePart2(string inputFile);

    protected dynamic Solve(string inputFile, int part)
    {
        return part switch
        {
            1 => SolvePart1(inputFile),
            2 => SolvePart2(inputFile),
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Part must be 1 or 2")
        };
    }

    [Theory]
    [InlineData("Sample1.txt")]
    [InlineData("Input.txt")]
    public void Part1(string relativeFilePath)
    {
        var inputFile = $"{GetType().Name}\\{relativeFilePath}";
        var expectedAnswer = GetExpectedAnswer(relativeFilePath, 1);
        var calculatedAnswer = Solve(inputFile, 1);
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    [Theory]
    [InlineData("Sample2.txt"), ]
    [InlineData("Input.txt")]
    public void Part2(string relativeFilePath)
    {
        var inputFile = $"{GetType().Name}\\{relativeFilePath}";
        var expectedAnswer = GetExpectedAnswer(relativeFilePath, 2);
        var calculatedAnswer = Solve(inputFile, 2);
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    private dynamic GetExpectedAnswer(string relativeFilePath, int part)
    {
        return part switch
        {
            1 => relativeFilePath switch
            {
                "Sample1.txt" => Part1Answers.SampleAnswer,
                "Input.txt" => Part1Answers.InputAnswer,
                _ => throw new ArgumentOutOfRangeException(nameof(relativeFilePath), relativeFilePath, "Invalid file path")
            },
            2 => relativeFilePath switch
            {
                "Sample2.txt" => Part2Answers.SampleAnswer,
                "Input.txt" => Part2Answers.InputAnswer,
                _ => throw new ArgumentOutOfRangeException(nameof(relativeFilePath), relativeFilePath, "Invalid file path")
            },
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Part must be 1 or 2")
        };
    }
}