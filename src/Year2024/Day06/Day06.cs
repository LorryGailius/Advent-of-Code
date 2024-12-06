using Utils;

namespace Year2024.Day06;

// https://adventofcode.com/2024/day/6

public class Day06 : BaseDay
{
    protected override Answers Part1Answers => new(41, 5030);
    protected override Answers Part2Answers => new(6, 1928);

    protected override int SolvePart1(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var guard = GetGuard(map);

        return GetPointsVisited(map, guard).Count;
    }

    // Takes a while to execute on the input file
    protected override int SolvePart2(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var guard = GetGuard(map);

        return GetAvailableObstaclePositions(map, guard).Count;
    }

    private static List<Point> GetPointsVisited(char[][] map, Guard guard)
    {
        while (true)
        {
            var row = guard.Position.Y;
            var col = guard.Position.X;

            switch (guard.Direction)
            {
                case Direction.Up:
                    row--;
                    break;
                case Direction.Down:
                    row++;
                    break;
                case Direction.Left:
                    col--;
                    break;
                case Direction.Right:
                    col++;
                    break;
            }

            if (row < 0 || row >= map.Length || col < 0 || col >= map[0].Length)
            {
                break;
            }

            if (map[row][col] == '#')
            {
                guard.Turn();
            }
            else
            {
                guard.Move();
            }
        }

        return guard.Path.DistinctBy(p => (p.X, p.Y)).ToList();
    }

    private static List<Point> GetAvailableObstaclePositions(char[][] map, Guard guard)
    {
        List<Point> availableObstaclePositions = [];

        var availablePoints = GetPointsVisited(map, new Guard(guard.Position, guard.Path.ToList())).DistinctBy(x => (x.X, x.Y)).Except([guard.Path[0]]);

        foreach (var availableObstaclePosition in availablePoints)
        {
            // create a copy of the map and the guard
            var guardCopy = new Guard(guard.Position, []);

            if (IsObstacleCausingLoop(map, availableObstaclePosition, guardCopy))
            {
                availableObstaclePositions.Add(availableObstaclePosition);
            }
        }

        return availableObstaclePositions;
    }

    private static bool IsObstacleCausingLoop(char[][] map, Point obstaclePoint, Guard guard)
    {
        map[obstaclePoint.Y][obstaclePoint.X] = 'O';

        while (true)
        {
            var row = guard.Position.Y;
            var col = guard.Position.X;

            switch (guard.Direction)
            {
                case Direction.Up:
                    row--;
                    break;
                case Direction.Down:
                    row++;
                    break;
                case Direction.Left:
                    col--;
                    break;
                case Direction.Right:
                    col++;
                    break;
            }

            if (row < 0 || row >= map.Length || col < 0 || col >= map[0].Length)
            {
                break;
            }

            var symbol = map[row][col];

            if (map[row][col] is '#' or 'O')
            {
                if(guard.Path.Count(x => x == guard.Position with { Direction = guard.Direction }) > 1)
                {
                    map[obstaclePoint.Y][obstaclePoint.X] = '.';
                    return true;
                }

                guard.Turn();
            }
            else
            {
                guard.Move();
            }
        }
        map[obstaclePoint.Y][obstaclePoint.X] = '.';
        return false;
    }

    private Guard GetGuard(char[][] map)
    {
        // search for a ^ symbol in the map
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '^')
                {
                    var startingPoint = new Point(x, y, Direction.Up);
                    return new Guard(startingPoint,[startingPoint]);
                }
            }
        }

        throw new Exception("No guard found");
    }

    internal class Guard(Point position, List<Point> path)
    {
        public Point Position { get; set; } = position;
        public Direction Direction { get; set; } = Direction.Up;
        public List<Point> Path { get; set; } = path;

        public void Turn()
        {
            Direction = (Direction)(((int)Direction + 1) % 4);
        }

        public void Move()
        {
            switch (Direction)
            {
                case Direction.Up:
                    Position = new Point(Position.X, Position.Y - 1, Direction);
                    break;
                case Direction.Down:
                    Position = new Point(Position.X, Position.Y + 1, Direction);
                    break;
                case Direction.Left:
                    Position = new Point(Position.X - 1, Position.Y, Direction);
                    break;
                case Direction.Right:
                    Position = new Point(Position.X + 1, Position.Y, Direction);
                    break;
            }

            Path.Add(Position);
        }
    }

    internal record Point(int X, int Y, Direction? Direction = null);

    internal enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}