using Utils;

namespace Year2024.Day04;

// https://adventofcode.com/2024/day/4

public class Day04 : BaseDay
{
    protected override Answers Part1Answers { get; } = new(18, 2560);
    protected override Answers Part2Answers { get; } = new(9, 1910);
    protected override int SolvePart1(string inputFile)
    {
        var matrix = Input.ToMatrix(inputFile);

        return FindWord(matrix, "XMAS").Count;
    }

    protected override int SolvePart2(string inputFile)
    {
        var matrix = Input.ToMatrix(inputFile);

        return FindPatterns(matrix).Count;
    }

    private static List<(int, int)> FindWord(char[][] board, string word)
    {
        var rows = board.Length;
        var positions = new List<(int, int)>();

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

        for (var row = 0; row < rows; row++)
        {
            var cols = board[row].Length;
            for (var col = 0; col < cols; col++)
            {
                if (board[row][col] == word[0])
                {
                    for (var d = 0; d < directions.GetLength(0); d++)
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

    private static List<(int, int)> FindPatterns(char[][] board)
    {
        var rows = board.Length;
        var positions = new List<(int, int)>();

        for (var row = 0; row < rows; row++)
        {
            var cols = board[row].Length;
            for (var col = 0; col < cols; col++)
            {
                if (row + 2 < rows && col + 2 < cols)
                {
                    char[][] pattern =
                    [
                        [board[row][col], board[row][col + 1], board[row][col + 2]],
                        [board[row + 1][col], board[row + 1][col + 1], board[row + 1][col + 2]],
                        [board[row + 2][col], board[row + 2][col + 1], board[row + 2][col + 2]]
                    ];

                    if (IsPatternCorrect(pattern))
                    {
                        positions.Add((row, col));
                    }
                }
            }
        }

        return positions;
    }

    private static bool IsPatternCorrect(char[][] pattern)
    {
        const string patternMatch = "MAS";
        
        var word1 = $"{pattern[0][0]}{pattern[1][1]}{pattern[2][2]}";
        var word2 = $"{pattern[2][0]}{pattern[1][1]}{pattern[0][2]}";

        var isWord1Correct = word1 == patternMatch || word1.ReverseString() == patternMatch;
        var isWord2Correct = word2 == patternMatch || word2.ReverseString() == patternMatch;

        return isWord1Correct && isWord2Correct;
    }
}