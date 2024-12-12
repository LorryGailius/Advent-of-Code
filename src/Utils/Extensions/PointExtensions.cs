namespace Utils.Extensions;

public static class PointExtensions
{
    public static IEnumerable<Point2D> ToPoints(this char[][] map)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                yield return new Point2D(x, y);
            }
        }
    }
}