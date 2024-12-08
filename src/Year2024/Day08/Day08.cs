using Utils;

namespace Year2024.Day08;

public class Day08 : BaseDay
{
    protected override Answers Part1Answers => new(14, 261);
    protected override Answers Part2Answers => new(34, 898);
    protected override dynamic SolvePart1(string inputFile)
    {
        var matrix = Input.ToMatrix(inputFile);

        var satellites = GetSatellites(matrix);

        var satelliteGroups = GetSatelliteGroups(satellites, matrix[0].Length - 1, matrix.Length - 1);

        return satelliteGroups.SelectMany(x => x.Antinodes).Distinct().Count();
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var matrix = Input.ToMatrix(inputFile);

        var satellites = GetSatellites(matrix);

        var satelliteGroups = GetSatelliteGroups(satellites, matrix[0].Length - 1, matrix.Length - 1, true);

        return satelliteGroups.SelectMany(x => x.Antinodes).Distinct().Count();
    }

    private List<SatelliteGroup> GetSatelliteGroups(List<Satellite> satellites, int lengthX, int lengthY, bool repeating = false)
    {
        var satelliteGroups = new List<SatelliteGroup>();
        for (var i = 0; i < satellites.Count; i++)
        {
            for (var j = i + 1; j < satellites.Count; j++)
            {
                if (satellites[i].Id != satellites[j].Id)
                {
                    continue;
                }

                var antinodes = GetAntinodes(satellites[i], satellites[j], lengthX, lengthY, repeating);
                satelliteGroups.Add(new SatelliteGroup(satellites[i], satellites[j], antinodes));
            }
        }
        return satelliteGroups;
    }

    private List<Point> GetAntinodes(Satellite sat1, Satellite sat2, int lengthX, int lengthY, bool repeating = false)
    {
        List<Point> antinodes = [];

        var diff = new Point(
            Math.Abs(sat1.Position.X - sat2.Position.X),
            Math.Abs(sat1.Position.Y - sat2.Position.Y)
        );

        if (diff.X == 0 || diff.Y == 0)
        {
            return [];
        }

        var direction = new Point(
            sat1.Position.X < sat2.Position.X ? 1 : -1,
            sat1.Position.Y < sat2.Position.Y ? 1 : -1
        );

        if (!repeating)
        {
            var antinode1 = CreateOffset(sat1.Position, direction, diff * 2);
            var antinode2 = CreateOffset(sat2.Position, direction, -diff * 2);

            if (antinode1.X >= 0 && antinode1.X <= lengthX && antinode1.Y >= 0 && antinode1.Y <= lengthY)
            {
                antinodes.Add(antinode1);
            }

            if (antinode2.X >= 0 && antinode2.X <= lengthX && antinode2.Y >= 0 && antinode2.Y <= lengthY)
            {
                antinodes.Add(antinode2);
            }
        }
        else
        {
            for (var i = 1; ; i++)
            {
                var newAntinode = CreateOffset(
                    sat1.Position,
                    direction,
                    diff * i
                );

                if (!IsPointWithinBounds(newAntinode, lengthX, lengthY))
                {
                    break;
                }

                antinodes.Add(newAntinode);
            }

            for (var i = 1; ; i++)
            {
                var newAntinode = CreateOffset(
                    sat2.Position,
                    -direction,
                    diff * i
                );

                if (!IsPointWithinBounds(newAntinode, lengthX, lengthY))
                {
                    break;
                }

                antinodes.Add(newAntinode);
            }
        }

        return antinodes;
    }

    private List<Satellite> GetSatellites(char[][] matrix)
    {
        var satellites = new List<Satellite>();
        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                if (matrix[i][j] == '.')
                {
                    continue;
                }
                satellites.Add(new Satellite(matrix[i][j], new Point(j, i)));
            }
        }
        return satellites;
    }

    private static Point CreateOffset(Point position, Point direction, Point amount)
    {
        return new Point(
            position.X + direction.X * amount.X,
            position.Y + direction.Y * amount.Y
        );
    }


    private static bool IsPointWithinBounds(Point point, int lengthX, int lengthY)
    {
        return point.X >= 0 && point.X <= lengthX && point.Y >= 0 && point.Y <= lengthY;
    }

    internal record SatelliteGroup(
        Satellite Sat1,
        Satellite Sat2,
        List<Point> Antinodes
    );

    internal record Satellite(
        char Id,
        Point Position
    );

    internal record Point(
        int X,
        int Y
    )
    {
        public static Point operator -(Point p) => new(-p.X, -p.Y);

        public static Point operator *(Point p, int n) => new(p.X * n, p.Y * n);
    };
}