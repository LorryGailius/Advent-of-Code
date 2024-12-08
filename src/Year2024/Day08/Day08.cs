using Utils;

namespace Year2024.Day08;

// https://adventofcode.com/2024/day/8

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

    private List<Point2D> GetAntinodes(Satellite sat1, Satellite sat2, int lengthX, int lengthY, bool repeating = false)
    {
        List<Point2D> antinodes = [];

        var diff = sat1.Position - sat2.Position;

        if (diff.X == 0 || diff.Y == 0)
        {
            return [];
        }

        if (!repeating)
        {
            var antinode1 = sat1.Position - diff * 2;
            var antinode2 = sat2.Position + diff * 2;

            if (antinode1.IsWithinBounds((lengthX, lengthY)))
            {
                antinodes.Add(antinode1);
            }

            if (antinode2.IsWithinBounds((lengthX, lengthY)))
            {
                antinodes.Add(antinode2);
            }
        }
        else
        {
            for (var i = 1; ; i++)
            {
                var newAntinode = sat1.Position - diff * i;

                if(!newAntinode.IsWithinBounds(new Point2D(lengthX, lengthY)))
                {
                    break;
                }

                antinodes.Add(newAntinode);
            }

            for (var i = 1; ; i++)
            {
                var newAntinode = sat2.Position + diff * i;

                if (!newAntinode.IsWithinBounds(new Point2D(lengthX, lengthY)))
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

                var id = matrix[i][j];
                Point2D position = new(j, i);

                satellites.Add(new (id, position));
            }
        }
        return satellites;
    }

    internal record SatelliteGroup(
        Satellite Sat1,
        Satellite Sat2,
        List<Point2D> Antinodes
    );

    internal record Satellite(
        char Id,
        Point2D Position
    );
}