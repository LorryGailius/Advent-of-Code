namespace Utils;

public static class Input
{
    public static string[] ToLines(string filename)
    {
        var assemblyDirectory = Path.GetDirectoryName(typeof(Input).Assembly.Location)!;

        var projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Could not determine project directory.");

        var path = Path.Combine(projectDirectory, filename);
        var lines = File.ReadAllLines(path);
        return lines;
    }

    public static string ToString(string filename)
    {
        var assemblyDirectory = Path.GetDirectoryName(typeof(Input).Assembly.Location)!;

        var projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Could not determine project directory.");

        var path = Path.Combine(projectDirectory, filename);
        var text = File.ReadAllText(path);
        return text;
    }

    public static List<List<T>> ToLists<T>(string filename) 
    {
        var assemblyDirectory = Path.GetDirectoryName(typeof(Input).Assembly.Location)!;

        var projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Could not determine project directory.");

        var path = Path.Combine(projectDirectory, filename);

        using var stream = new StreamReader(path);

        var result = new List<List<T>>();

        var line = stream.ReadLine();
        var values = line.SplitTo<T>();
        var valueCount = values.Length;

        Enumerable.Range(0, valueCount).ToList().ForEach(_ => result.Add([]));

        result.Select((list, index) => (list, index)).ToList().ForEach(x => x.list.Add(values[x.index]));

        while ((line = stream.ReadLine()) != null)
        {
            values = line.SplitTo<T>();
            result.Select((list, index) => (list, index)).ToList().ForEach(x => x.list.Add(values[x.index]));
        }

        return result;
    }

    public static char[][] ToMatrix(string filename)
    {
        var assemblyDirectory = Path.GetDirectoryName(typeof(Input).Assembly.Location)!;

        var projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Could not determine project directory.");

        var path = Path.Combine(projectDirectory, filename);
        using var stream = new StreamReader(path);

        var result = new List<char[]>();
        string line;
        while ((line = stream.ReadLine()) != null)
        {
            result.Add(line.ToCharArray());
        }
        return result.ToArray();
    }
}