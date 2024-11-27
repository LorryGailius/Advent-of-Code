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
}