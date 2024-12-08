using System.Diagnostics;

namespace Utils;

[DebuggerDisplay("({X}, {Y})")]
public class Point2D
{
    public readonly int X;
    public readonly int Y;

    public int Manhattan => Math.Abs(X) + Math.Abs(Y);

    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point2D(string x, string y)
    {
        X = int.Parse(x);
        Y = int.Parse(y);
    }

    public int ManhattanDistanceFrom(in Point2D p)
    {
        return Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
    }

    public bool IsWithinBounds(Point2D bounds)
    {
        return X >= 0 && X <= bounds.X && Y >= 0 && Y <= bounds.Y;
    }

    public IEnumerable<Point2D> Adjacent()
    {
        yield return new Point2D(X + 1, Y);
        yield return new Point2D(X, Y + 1);
        yield return new Point2D(X - 1, Y);
        yield return new Point2D(X, Y - 1);
    }

    public IEnumerable<Point2D> Adjacent(Point2D bounds)
    {
        if (X > 0)            yield return new Point2D(X - 1, Y);
        if (X < bounds.X - 1) yield return new Point2D(X + 1, Y);
        if (Y > 0)            yield return new Point2D(X, Y - 1);
        if (Y < bounds.Y - 1) yield return new Point2D(X, Y + 1);
    }

    public Point2D Transform(Func<int, int> transform)
    {
        return new Point2D(transform(X), transform(Y));
    }

    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    // Equality
    public override bool Equals(object? obj)
    {
        return obj is Point2D point && Equals(point);
    }

    public bool Equals(Point2D other)
    {
        return X == other.X &&
               Y == other.Y;
    }

    public static bool operator ==(Point2D a, Point2D b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Point2D a, Point2D b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static Point2D Min(IEnumerable<Point2D> points)
    {
        int minX, minY;
        minX = minY = int.MaxValue;
        foreach (Point2D point in points)
        {
            if (point.X < minX) minX = point.X;
            if (point.Y < minY) minY = point.Y;
        }

        return new Point2D(minX, minY);
    }

    public static Point2D Max(IEnumerable<Point2D> points)
    {
        int maxX, maxY;
        maxX = maxY = int.MinValue;
        foreach (Point2D point in points)
        {
            if (point.X > maxX) maxX = point.X;
            if (point.Y > maxY) maxY = point.Y;
        }

        return new Point2D(maxX, maxY);
    }

    public static (Point2D min, Point2D max) MinMax(IEnumerable<Point2D> points)
    {
        int minX, minY, maxX, maxY;
        minX = minY = int.MaxValue;
        maxX = maxY = int.MinValue;
        foreach (Point2D point in points)
        {
            if (point.X < minX) minX = point.X;
            if (point.X > maxX) maxX = point.X;
            if (point.Y < minY) minY = point.Y;
            if (point.Y > maxY) maxY = point.Y;
        }

        return (new Point2D(minX, minY), new Point2D(maxX, maxY));
    }

    // Static members
    public static Point2D Zero => new Point2D(0, 0);

    // Operators
    public static Point2D operator +(Point2D a) => a;
    public static Point2D operator -(Point2D a) => new Point2D(-a.X, -a.Y);
    public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
    public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
    public static Point2D operator *(Point2D a, int b) => new Point2D(a.X * b, a.Y * b);
    public static Point2D operator *(Point2D a, Point2D b) => new Point2D(a.X * b.X, a.Y * b.Y);
    public static Point2D operator /(Point2D a, int b) => new Point2D(a.X / b, a.Y / b);
    public static Point2D operator /(Point2D a, Point2D b) => new Point2D(a.X / b.X, a.Y / b.Y);
    public Point2D Abs() => new Point2D(Math.Abs(X), Math.Abs(Y));

    // Conversions
    public static implicit operator (int, int)(Point2D pair) => (pair.X, pair.Y);
    public static implicit operator Point2D((int x, int y) pair) => new Point2D(pair.x, pair.y);
    public static implicit operator Point2D((string x, string y) pair) => new Point2D(pair.x, pair.y);
}