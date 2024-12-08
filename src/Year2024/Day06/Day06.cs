using Utils;

namespace Year2024.Day06;

// https://adventofcode.com/2024/day/6

public class Day06 : BaseDay
{
    protected override Answers Part1Answers => new(41, 5030);
    protected override Answers Part2Answers => new(6, 1928);

    protected override dynamic SolvePart1(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var guard = GetGuard(map);

        return GetPointsVisited(map, guard).Count;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var guard = GetGuard(map);

        return GetAvailableObstaclePositions(map, guard).Count;
    }

    private static List<VectorPoint> GetPointsVisited(char[][] map, Guard guard)
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

        return guard.Path.DistinctBy(p => p.Coordinates).ToList();
    }

    private static List<Point2D> GetAvailableObstaclePositions(char[][] map, Guard guard)
    {
        List<Point2D> availableObstaclePositions = [];

        var availablePoints = GetPointsVisited(map, new Guard(guard.Position, guard.Path)).Skip(1);

        foreach (var availableObstaclePosition in availablePoints)
        {
            var newGuard = new Guard(guard.Position, []);

            if (IsObstacleCausingLoop(map, availableObstaclePosition.Coordinates, newGuard))
            {
                availableObstaclePositions.Add(availableObstaclePosition.Coordinates);
            }
        }

        return availableObstaclePositions;
    }

    private static bool IsObstacleCausingLoop(char[][] map, Point2D obstaclePoint, Guard guard)
    {
        map[obstaclePoint.Y][obstaclePoint.X] = 'O';

        var previousDirections = new List<VectorPoint>();

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

            if (map[row][col] is '#' or 'O')
            {
                if (previousDirections.Any(p => p.Coordinates == guard.Position && p.Direction == guard.Direction))
                {
                    map[obstaclePoint.Y][obstaclePoint.X] = '.';
                    return true;
                }

                previousDirections.Add(new(guard.Position, guard.Direction));
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
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '^')
                {
                    var startingPoint = new VectorPoint((x, y), Direction.Up);
                    return new Guard(startingPoint,[startingPoint]);
                }
            }
        }

        throw new Exception("No guard found");
    }

    internal class Guard()
    {
        public Point2D Position { get; set; }
        public Direction Direction { get; set; } = Direction.Up;
        public List<VectorPoint> Path { get; set; }

        public Guard(VectorPoint position, List<VectorPoint> path) : this()
        {
            this.Position = position.Coordinates;
            this.Direction = Direction.Up;
            this.Path = path;
        }

        public Guard(Point2D position, List<VectorPoint> path) : this()
        {
            this.Position = position;
            this.Path = path;
        }

        public void Turn()
        {
            Direction = (Direction)(((int)Direction + 1) % 4);
        }

        public void Move()
        {
            switch (Direction)
            {
                case Direction.Up:
                    Position += new Point2D(0, -1);
                    break;
                case Direction.Down:
                    Position += new Point2D(0, 1);
                    break;
                case Direction.Left:
                    Position += new Point2D(-1, 0);
                    break;
                case Direction.Right:
                    Position += new Point2D(1, 0);
                    break;
            }

            Path.Add(new(Position, Direction));
        }
    }

    internal record VectorPoint(Point2D Coordinates, Direction? Direction = null);

    internal enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}