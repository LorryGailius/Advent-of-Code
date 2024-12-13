using Utils;
using Utils.Extensions;

namespace Year2024.Day12;

public class Day12 : BaseDay
{
    protected override Answers Part1Answers => new(1930, 1434856);
    protected override Answers Part2Answers => new(1206, 891106);
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
        var map = Input.ToMatrix(inputFile);

        var points = map.ToPoints();
        HashSet<Point2D> visited = [];

        var result = 0L;

        foreach (var point in points)
        {
            result += GetPrice(map, point, visited, true);
        }

        return result;
    }

    private static long GetPrice(char[][] map, Point2D p, HashSet<Point2D> visited, bool countSides = false)
    {
        if (visited.Contains(p))
        {
            return 0;
        }

        var symbol = map[p.Y][p.X];

        if (countSides)
        {
            var areaSides = GetAreaAndSides(map, p, visited, symbol);
            return areaSides.Area * areaSides.Sides;
        }

        var areaPerimetere = GetAreaAndPerimeter(map, p, visited, symbol);
        return areaPerimetere.Area * areaPerimetere.Perimeter;
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

    private static (long Area, long Sides) GetAreaAndSides(char[][] map, Point2D p, HashSet<Point2D> visited, char symbol)
    {
        var area = 0;
        var sides = 0;

        var stack = new Stack<Point2D>();
        stack.Push(p);

        Dictionary<Direction, HashSet<Point2D>> perimeterCollisionPoints = [];

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
                        var direction = GetDirection(current, adjacent);

                        if (!perimeterCollisionPoints.ContainsKey(direction))
                        {
                            perimeterCollisionPoints[direction] = [];
                        }

                        perimeterCollisionPoints[direction].Add(current);
                    }
                }
                else
                {
                    var direction = GetDirection(current, adjacent);

                    if (!perimeterCollisionPoints.ContainsKey(direction))
                    {
                        perimeterCollisionPoints[direction] = [];
                    }

                    perimeterCollisionPoints[direction].Add(current);
                }
            }
        }

        foreach (var direction in perimeterCollisionPoints.Keys)
        {
            HashSet<Point2D> visitedCollisions = [];
            var directionMap = new char[map.Length][];

            for (var i = 0; i < map.Length; i++)
            {
                directionMap[i] = new char[map[i].Length];
                for (var j = 0; j < map[i].Length; j++)
                {
                    directionMap[i][j] = '.';
                }
            }

            foreach (var point in perimeterCollisionPoints[direction])
            {
                directionMap[point.Y][point.X] = symbol;
            }

            foreach (var point in perimeterCollisionPoints[direction])
            {
                if (visitedCollisions.Contains(point))
                {
                    continue;
                }

                sides++;
                var newStack = new Stack<Point2D>();
                newStack.Push(point);
                while (newStack.Count > 0)
                {
                    var current = newStack.Pop();
                    if (directionMap[current.Y][current.X] != symbol)
                    {
                        continue;
                    }
                    if (!visitedCollisions.Add(current))
                    {
                        continue;
                    }
                    foreach (var adjacent in current.Adjacent())
                    {
                        if (adjacent.IsWithinBounds(new Point2D(map[0].Length - 1, map.Length - 1)))
                        {
                            if (directionMap[adjacent.Y][adjacent.X] == symbol)
                            {
                                newStack.Push(adjacent);
                            }
                        }
                    }
                }
            }
        }

        return (area, sides);
    }

    private static Direction GetDirection(Point2D current, Point2D adjacent)
    {
        if (current.X == adjacent.X)
        {
            if (current.Y > adjacent.Y)
            {
                return Direction.Up;
            }

            return Direction.Down;
        }

        if (current.X > adjacent.X)
        {
            return Direction.Left;
        }

        return Direction.Right;
    }

    internal enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}