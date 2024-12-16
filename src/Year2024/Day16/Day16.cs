using Utils;

namespace Year2024.Day16;

public class Day16 : BaseDay
{
    protected override Answers Part1Answers => new(7036, 106512);
    protected override Answers Part2Answers => new(64, 563);
    protected override dynamic SolvePart1(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var reinDeer = GetDeer(map);

        var result = Navigate(map, reinDeer);

        return result.bestScore;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var map = Input.ToMatrix(inputFile);
        var reinDeer = GetDeer(map);

        var result = Navigate(map, reinDeer);

        var allVisitedPoints = result.paths.SelectMany(x => x).Select(x => x.Position).Distinct().Count();

        return allVisitedPoints;
    }

    private static (long bestScore, List<List<Vec2D>> paths) Navigate(char[][] map, Deer deer)
    {
        var startingVector = deer.Pose;
        var start = new State(startingVector, 0, [startingVector]);
        var queue = new Queue<State>([start]);

        var bestScore = long.MaxValue;
        List<List<Vec2D>> bestPaths = [];
        Dictionary<Vec2D, long> minScores = [];

        while (queue.Count != 0)
        {
            var state = queue.Dequeue();
            if (state.Score > bestScore)
            {
                continue;
            }

            if(state.Pose.Position.X < 0 || state.Pose.Position.X >= map[0].Length || state.Pose.Position.Y < 0 || state.Pose.Position.Y >= map.Length)
            {
                continue;
            }

            if (map[state.Pose.Position.Y][state.Pose.Position.X] == 'E')
            {
                state.Path.Add(state.Pose);

                if (state.Score == bestScore)
                {
                    bestPaths.Add(state.Path);
                }
                else if (state.Score < bestScore)
                {
                    bestScore = state.Score;
                    bestPaths = [state.Path];
                }
                continue;
            }

            if (map[state.Pose.Position.Y][state.Pose.Position.X] != '#')
            {
                EnqueueIfBetter(state.Step(), minScores, queue);
            }

            EnqueueIfBetter(state.Left(), minScores, queue);
            EnqueueIfBetter(state.Right(), minScores, queue);
        }

        return (bestScore, bestPaths);
    }

    private static void EnqueueIfBetter(State candidate, Dictionary<Vec2D, long> minScores, Queue<State> queue)
    {
        var score = minScores.GetValueOrDefault(candidate.Pose, int.MaxValue);

        if (score >= candidate.Score)
        {
            minScores[candidate.Pose] = candidate.Score;
            queue.Enqueue(candidate);
        }
    }

    internal record State(Vec2D Pose, int Score, List<Vec2D> Path)
    {
        public State Step() => new(Pose.Step(), Score + 1, [..Path, Pose]);
        public State Left() => new(Pose.Turn(-1), Score + 1000, Path);
        public State Right() => new(Pose.Turn(1), Score + 1000, Path);
    };

    private static Deer GetDeer(char[][] map)
    {
        var lengthX = map[0].Length;
        var lengthY = map.Length;
        for (var y = 0; y < lengthY; y++)
        {
            for (var x = 0; x < lengthX; x++)
            {
                if (map[y][x] == 'S')
                {
                    return new Deer(new Vec2D((x, y), Direction.East));
                }
            }
        }

        throw new InvalidOperationException("No deer found");
    }

    internal record Deer(Vec2D Pose);

    internal record Vec2D(Point2D Position, Direction Direction)
    {
        public Vec2D Step() => Direction switch
        {
            Direction.North => this with { Position = new Point2D(Position.X, Position.Y - 1) },
            Direction.East => this with { Position = new Point2D(Position.X + 1, Position.Y) },
            Direction.South => this with { Position = new Point2D(Position.X, Position.Y + 1) },
            Direction.West => this with { Position = new Point2D(Position.X - 1, Position.Y) },
            _ => throw new ArgumentOutOfRangeException()
        };

        public Vec2D Turn(int direction) => this with { Direction = (Direction)(((int)Direction + direction + 4) % 4) };
    }
}

internal enum Direction
{
    North,
    East,
    South,
    West
}