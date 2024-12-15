using System.Text;
using Utils;

namespace Year2024.Day15;

// https://adventofcode.com/2024/day/15

public class Day15 : BaseDay
{
    protected override Answers Part1Answers => new(10092, 1318523);
    protected override Answers Part2Answers => new(9021, 1337648);
    protected override dynamic SolvePart1(string inputFile)
    {
        var (map, bot, moveSet) = ReadInput(inputFile);

        foreach (var move in moveSet)
        {
            if (CanMove(bot, move, map))
            {
                Move(bot, move, map);
            }
        }

        var result = map.Objects.Values.OfType<Box>().Sum(box => box.Position.Y * 100 + box.Position.X);

        return result;
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var (map, bot, moveSet) = ReadInput(inputFile, true);

        foreach (var move in moveSet)
        {
            if(CanMove(bot, move, map))
            {
                Move(bot, move, map);
            }
        }

        var result = map.Objects.Values.OfType<BigBox>().GroupBy(x => (x.Position, x.Position2))
            .Sum(x => x.Key.Position.Y * 100 + x.Key.Position.X);

        return result;
    }

    private static bool CanMove(MapObject obj, Direction direction, Map map)
    {
        var newPosition = direction switch
        {
            Direction.Up => new Point2D(obj.Position.X, obj.Position.Y - 1),
            Direction.Down => new Point2D(obj.Position.X, obj.Position.Y + 1),
            Direction.Left => new Point2D(obj.Position.X - 1, obj.Position.Y),
            Direction.Right => new Point2D(obj.Position.X + 1, obj.Position.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        if (obj is BigBox bigBox)
        {
            newPosition = direction switch
            {
                Direction.Up => new Point2D(bigBox.Position.X, bigBox.Position.Y - 1),
                Direction.Down => new Point2D(bigBox.Position.X, bigBox.Position.Y + 1),
                Direction.Left => new Point2D(bigBox.Position.X - 1, bigBox.Position.Y),
                Direction.Right => new Point2D(bigBox.Position.X + 1, bigBox.Position.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            var newPosition1 = direction switch
            {
                Direction.Up => new Point2D(bigBox.Position2.X, bigBox.Position2.Y - 1),
                Direction.Down => new Point2D(bigBox.Position2.X, bigBox.Position2.Y + 1),
                Direction.Left => new Point2D(bigBox.Position2.X - 1, bigBox.Position2.Y),
                Direction.Right => new Point2D(bigBox.Position2.X + 1, bigBox.Position2.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if(newPosition != bigBox.Position2 && map.Objects.TryGetValue(newPosition, out var objectInNewPosition1))
            {
                if (objectInNewPosition1 is Wall || !CanMove(objectInNewPosition1, direction, map))
                {
                    return false;
                }
            }

            if (newPosition1 != bigBox.Position && map.Objects.TryGetValue(newPosition1, out var objectInNewPosition2))
            {
                if (objectInNewPosition2 is Wall || !CanMove(objectInNewPosition2, direction, map))
                {
                    return false;
                }
            }

            return true;
        }

        if (map.Objects.TryGetValue(newPosition, out var objectInNewPosition))
        {
            if (objectInNewPosition is Wall || !CanMove(objectInNewPosition, direction, map))
            {
                return false;
            }
        }

        return true;
    }

    private static void Move(MapObject obj, Direction direction, Map map)
    {
        var newPosition = direction switch
        {
            Direction.Up => new Point2D(obj.Position.X, obj.Position.Y - 1),
            Direction.Down => new Point2D(obj.Position.X, obj.Position.Y + 1),
            Direction.Left => new Point2D(obj.Position.X - 1, obj.Position.Y),
            Direction.Right => new Point2D(obj.Position.X + 1, obj.Position.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        if (obj is BigBox bigBox)
        {
            newPosition = direction switch
            {
                Direction.Up => new Point2D(bigBox.Position.X, bigBox.Position.Y - 1),
                Direction.Down => new Point2D(bigBox.Position.X, bigBox.Position.Y + 1),
                Direction.Left => new Point2D(bigBox.Position.X - 1, bigBox.Position.Y),
                Direction.Right => new Point2D(bigBox.Position.X + 1, bigBox.Position.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            var newPosition1 = direction switch
            {
                Direction.Up => new Point2D(bigBox.Position2.X, bigBox.Position2.Y - 1),
                Direction.Down => new Point2D(bigBox.Position2.X, bigBox.Position2.Y + 1),
                Direction.Left => new Point2D(bigBox.Position2.X - 1, bigBox.Position2.Y),
                Direction.Right => new Point2D(bigBox.Position2.X + 1, bigBox.Position2.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if (newPosition != bigBox.Position2 && map.Objects.TryGetValue(newPosition, out var objectInNewPosition1))
            {
                Move(objectInNewPosition1, direction, map);
            }

            if (newPosition1 != bigBox.Position && map.Objects.TryGetValue(newPosition1, out var objectInNewPosition2))
            {
                Move(objectInNewPosition2, direction, map);
            }

            map.Objects.Remove(bigBox.Position);
            map.Objects.Remove(bigBox.Position2);
            bigBox.Position = newPosition;
            bigBox.Position2 = newPosition1;
            map.Objects[newPosition] = bigBox;
            map.Objects[newPosition1] = bigBox;

            return;
        }

        if (map.Objects.TryGetValue(newPosition, out var objectInNewPosition))
        {
            Move(objectInNewPosition, direction, map);
        }

        map.Objects.Remove(obj.Position);
        obj.Position = newPosition;
        map.Objects[newPosition] = obj;
    }

    private static (Map map, Bot bot, List<Direction> moveSet) ReadInput(string inputFile, bool wideMap = false)
    {
        var lines = Input.ToLines(inputFile);
        var map = new Map();
        var bot = new Bot();
        var moveSet = new List<Direction>();

        var mapLines = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToList();
        var moveSetLines = lines.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1);

        if (wideMap)
        {
            var lengthX = mapLines.First().Length;
            var lengthY = mapLines.Count;

            var builder = new StringBuilder();

            for (var y = 0; y < lengthY; y++)
            {
                for (var x = 0; x < lengthX; x++)
                {
                    var symbol = mapLines[y][x];
                    switch (symbol)
                    {
                        case '#':
                            builder.Append("##");
                            break;
                        case 'O':
                            builder.Append("[]");
                            break;
                        case '@':
                            builder.Append("@.");
                            break;
                        default:
                            builder.Append("..");
                            break;
                    }
                }
                builder.AppendLine();
            }

            mapLines = builder.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        map.LengthX = mapLines.First().Length;
        map.LengthY = mapLines.Count;

        for (var y = 0; y < map.LengthY; y++)
        {
            var line = mapLines[y];
            for (var x = 0; x < map.LengthX; x++)
            {
                var c = line[x];
                var position = new Point2D(x, y);
                switch (c)
                {
                    case '#':
                        map.Objects[position] = new Wall { Position = position };
                        break;
                    case 'O':
                        map.Objects[position] = new Box { Position = position };
                        break;
                    case '@':
                        bot.Position = position;
                        break;
                    case '[':
                        var bigBox = new BigBox { Position = position, Position2 = new Point2D(x + 1, y) };
                        map.Objects[position] = bigBox;
                        map.Objects[bigBox.Position2] = bigBox;
                        break;
                }
            }
        }

        foreach (var line in moveSetLines)
        {
            foreach (var c in line)
            {
                switch (c)
                {
                    case '^':
                        moveSet.Add(Direction.Up);
                        break;
                    case 'v':
                        moveSet.Add(Direction.Down);
                        break;
                    case '<':
                        moveSet.Add(Direction.Left);
                        break;
                    case '>':
                        moveSet.Add(Direction.Right);
                        break;
                }
            }
        }

        return (map, bot, moveSet);
    }

    internal class Map
    {
        public int LengthX { get; set; }
        public int LengthY { get; set; }
        public Dictionary<Point2D, MapObject> Objects { get; set; } = new();

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(Bot? bot)
        {
            var builder = new StringBuilder();

            for (var y = 0; y < LengthY; y++)
            {
                for (var x = 0; x < LengthX; x++)
                {
                    var position = new Point2D(x, y);

                    if(bot != null && position == bot.Position)
                    {
                        builder.Append('@');
                        continue;
                    }

                    builder.Append(Objects.TryGetValue(position, out var obj) ? obj switch
                    {
                        Wall => '#',
                        Box => 'O',
                        Bot => '@',
                        BigBox => obj.Position == position ? '[' : ']',
                        _ => '.'
                    } : '.');
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }

    internal class MapObject
    {
        public Point2D Position { get; set; }
    }

    internal class Bot : MapObject
    {
    }

    internal class Box : MapObject
    {
    }

    internal class BigBox : MapObject
    {
        public Point2D Position2 { get; set; }
    }

    internal class Wall : MapObject
    {
    }

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
