using Utils;
using Utils.Extensions;

namespace Year2024.Day12;

public class Day12 : BaseDay
{
    protected override Answers Part1Answers => new(1930, 1434856);
    protected override Answers Part2Answers => new(0, 0);
    protected override dynamic SolvePart1(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);

        var points = map.ToPoints();
        HashSet<Point2D> visited = [];

        long result = 0;

        foreach (var point in points)
        {
           result += GetPrice(map, point, visited);
        }

        return result;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        throw new NotImplementedException();
    }

    private static long GetPrice(char[][] map, Point2D p, HashSet<Point2D> visited)
    {
        if (visited.Contains(p))
        {
            return 0;
        }

        var symbol = map[p.Y][p.X];

        var result = GetAreaAndPerimeter(map, p, visited, symbol);
        return result.Area * result.Perimeter;
    }

    private static (long Area, long Perimeter) GetAreaAndPerimeter(char[][] map, Point2D p, HashSet<Point2D> visited, char symbol)
    {
        var area = 0;
        var perimeter = 0;

        var stack = new Stack<Point2D>();
        stack.Push(p);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (map[current.Y][current.X] != symbol)
            {
                continue;
            }

            if (!visited.Add(current))
            {
                continue;
            }

            area++;

            foreach (var adjacent in current.Adjacent())
            {
                if (adjacent.IsWithinBounds(new Point2D(map[0].Length - 1, map.Length - 1)))
                {
                    if (map[adjacent.Y][adjacent.X] == symbol)
                    {
                        stack.Push(adjacent);
                    }
                    else
                    {
                        perimeter++;
                    }
                }
                else
                {
                    perimeter++;
                }
            }
        }

        return (area, perimeter);
    }
}