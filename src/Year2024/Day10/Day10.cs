using Utils;

namespace Year2024.Day10;

// https://adventofcode.com/2024/day/10

public class Day10 : BaseDay
{
    protected override Answers Part1Answers => new(36, 652);
    protected override Answers Part2Answers => new(81, 1432);
    protected override dynamic SolvePart1(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var zeroPoints = FindZeroPoints(map);

        var score = 0;

        foreach (var point in zeroPoints)
        {
            score += GetPathScore(map, point, []);
        }

        return score;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var zeroPoints = FindZeroPoints(map);

        var rating = 0;

        foreach (var point in zeroPoints)
        {
            rating += GetPathRating(map, point);
        }

        return rating;
    }

    private static List<Point2D> FindZeroPoints(char[][] map)
    {
        var zeroPoints = new List<Point2D>();
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '0')
                {
                    zeroPoints.Add(new Point2D(x, y));
                }
            }
        }
        return zeroPoints;
    }

    private static int GetPathScore(char[][] map, Point2D currentPoint, HashSet<Point2D> visited)
    {
        visited.Add(currentPoint);

        if (map[currentPoint.Y][currentPoint.X] == '9')
        {
            return 1;
        }

        var bounds = new Point2D(map[0].Length, map.Length);

        var trails = 0;
        var adjacentPoints = currentPoint.Adjacent(bounds)
                .Where(x => !visited.Contains(x) &&
                            map[x.Y][x.X] == map[currentPoint.Y][currentPoint.X] + 1)
                .ToList();

        foreach (var point in adjacentPoints)
        {
            trails += GetPathScore(map, point, visited);
        }

        return trails;
    }

    private static int GetPathRating(char[][] map, Point2D currentPoint)
    {
        if (map[currentPoint.Y][currentPoint.X] == '9')
        {
            return 1;
        }

        var bounds = new Point2D(map[0].Length, map.Length);

        var trails = 0;
        var adjacentPoints = currentPoint.Adjacent(bounds)
            .Where(x => map[x.Y][x.X] == map[currentPoint.Y][currentPoint.X] + 1)
            .ToList();

        foreach (var point in adjacentPoints)
        {
            trails += GetPathRating(map, point);
        }

        return trails;
    }
}