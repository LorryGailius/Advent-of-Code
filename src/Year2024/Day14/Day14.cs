using System.Diagnostics;
using Utils;

namespace Year2024.Day14;

// https://adventofcode.com/2024/day/14
public class Day14 : BaseDay
{
    protected override Answers Part1Answers => new(21, 230172768);
    protected override Answers Part2Answers => new(0, 8087);
    protected override dynamic SolvePart1(string inputFile)
    {
        var width = 101;
        var height = 103;

        var bots = ParseInput(inputFile);

        var n = 100;

        foreach (var bot in bots)
        {
            bot.Move((width, height), n);
        }

        var midPoint = new Point2D(width / 2, height / 2);

        var botCount = CountForQuadrant((0, 0), (midPoint.X - 1, midPoint.Y - 1), bots) * 
                       CountForQuadrant((midPoint.X + 1, 0), (width, midPoint.Y - 1), bots) *
                       CountForQuadrant((0, midPoint.Y + 1), (midPoint.X - 1, height), bots) *
                       CountForQuadrant((midPoint.X + 1, midPoint.Y + 1), (width, height), bots);

        return botCount;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        // Part 2 is not solvable with the Sample input so return 0 
        if(inputFile.Contains("Sample", StringComparison.InvariantCultureIgnoreCase))
        {
            return 0;
        }

        var width = 101;
        var height = 103;

        var bots = ParseInput(inputFile);

        var seconds = 100L;

        foreach (var robot in bots)
        {
            robot.Move((width, height), (int)seconds);
        }

        while(true)
        {
            if (bots.Select(x => x.Position).Distinct().Count() == bots.Count)
            {
                break;
            }

            foreach (var robot in bots)
            {
                robot.Move((width, height));
            }

            seconds++;
        }

        return seconds;
    }

    private static List<Robot> ParseInput(string filePath)
    {
        var lines = Input.ToLines(filePath);

        List<Robot> bots = [];

        foreach (var line in lines)
        {
            var split = line.Split(" ")
                .Select(x => x.Split("="))
                .Select(x => x[1].Split(","))
                .Select(x => (Point2D)(x[0], x[1]))
                .ToList();

            bots.Add(new Robot(split[0], split[1]));
        }

        return bots;
    }

    [DebuggerDisplay("pos: ({Position.X}, {Position.Y}) vel: ({Velocity.X},{Velocity.Y})")]
    internal class Robot(Point2D position, Point2D velocity)
    {
        public Point2D Position { get; set; } = position;
        public Point2D Velocity { get; set; } = velocity;

        public void Move(Point2D bounds, int iterations = 1)
        {
            Position = ((Position.X + Velocity.X * iterations) % bounds.X,
                (Position.Y + Velocity.Y * iterations) % bounds.Y);

            if (Position.X < 0)
            {
                Position = (bounds.X + Position.X, Position.Y);
            }

            if (Position.Y < 0)
            {
                Position = (Position.X, bounds.Y + Position.Y);
            }
        }
    }

    private static int CountForQuadrant(Point2D leftBound, Point2D rightBound, List<Robot> bots)
    {
        return bots.Count(bot => bot.Position.X >= leftBound.X && bot.Position.X <= rightBound.X && bot.Position.Y >= leftBound.Y && bot.Position.Y <= rightBound.Y);
    }
}