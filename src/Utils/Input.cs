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

    public static List<List<T>> ToLists<T>(string filename) 
    {
        var assemblyDirectory = Path.GetDirectoryName(typeof(Input).Assembly.Location)!;

        var projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.FullName
                               ?? throw new InvalidOperationException("Could not determine project directory.");

        var path = Path.Combine(projectDirectory, filename);

        using var stream = new StreamReader(path);

        var result = new List<List<T>>();

        var line = stream.ReadLine();
        var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var valueCount = values.Length;

        Enumerable.Range(0, valueCount).ToList().ForEach(_ => result.Add([]));

        values.Select((value, index) => (value, index)).ToList().ForEach(x => result[x.index].Add((T)Convert.ChangeType(x.value, typeof(T))));

        while ((line = stream.ReadLine()) != null)
        {
            values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            values.Select((value, index) => (value, index)).ToList().ForEach(x => result[x.index].Add((T)Convert.ChangeType(x.value, typeof(T))));
        }

        return result;
    }
}