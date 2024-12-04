using Utils;

namespace Year2024.Day04;

public class Day04 : BaseDay
{
    protected override string Day { get; } = "Day04";
    protected override Answers Part1Answers { get; } = new(18, 2560);
    protected override Answers Part2Answers { get; } = new(9, 0);
    protected override int SolvePart1(string inputFile)
    {
        var matrix = Input.ToMatrix(inputFile);

        return FindWord(matrix, "XMAS").Count;
    }

    protected override int SolvePart2(string inputFile)
    {
        char[][] pattern = {
            new char[] { 'M', '.', 'S' },
            new char[] { '.', 'A', '.' },
            new char[] { 'M', '.', 'S' }
        };

        var matrix = Input.ToMatrix(inputFile);

        return FindPattern(matrix, pattern).Count;
    }



    static List<(int, int)> FindWord(char[][] board, string word)
    {
        int rows = board.Length;
        List<(int, int)> positions = new List<(int, int)>();

        int[,] directions = {
            { -1, 0 }, // Up
            { 1, 0 },  // Down
            { 0, -1 }, // Left
            { 0, 1 },  // Right
            { -1, -1 },// Diagonal Up-Left
            { -1, 1 }, // Diagonal Up-Right
            { 1, -1 }, // Diagonal Down-Left
            { 1, 1 }   // Diagonal Down-Right
        };

        for (int row = 0; row < rows; row++)
        {
            int cols = board[row].Length;
            for (int col = 0; col < cols; col++)
            {
                if (board[row][col] == word[0])
                {
                    for (int d = 0; d < directions.GetLength(0); d++)
                    {
                        int newRow = row, newCol = col;
                        int i;

                        for (i = 0; i < word.Length; i++)
                        {
                            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= board[newRow].Length || board[newRow][newCol] != word[i])
                                break;

                            newRow += directions[d, 0];
                            newCol += directions[d, 1];
                        }

                        if (i == word.Length)
                        {
                            positions.Add((row, col));
                        }
                    }
                }
            }
        }
        return positions;
    }
}