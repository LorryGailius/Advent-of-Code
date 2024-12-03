namespace Utils;

public static class StringExtensions
{
    public static int ToInt(this string s)
    {
        return int.Parse(s);
    }

    public static int[] ToIntArray(this string s)
    {
        return s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToArray();
    }

    public static long ToLong(this string s)
    {
        return long.Parse(s);
    }

    public static long[] ToLongArray(this string s)
    {
        return s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToArray();
    }
}