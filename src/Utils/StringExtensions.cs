namespace Utils;

public static class StringExtensions
{
    public static int ToInt(this string input)
    {
        return Convert.ToInt32(input);
    }

    public static long ToLong(this string input)
    {
        return Convert.ToInt64(input);
    }

    public static int[] SplitToIntegers(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt32(x.Trim())).ToArray();
    }

    public static long[] SplitToLongs(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x.Trim())).ToArray();
    }

    public static string[] SplitToStrings(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
    }

    public static T[] SplitTo<T>(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => (T)Convert.ChangeType(x.Trim(), typeof(T))).ToArray();
    }

    public static string ReverseString(this string s)
    {
        var charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static int ToInt(this char c)
    {
        return c.ToString().ToInt();
    }
}