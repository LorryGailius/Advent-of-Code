using Utils;

namespace Year2024.Day11;

public class Day11 : BaseDay
{
    protected override Answers Part1Answers => new(55312, 203953);
    protected override Answers Part2Answers => new(65601038650482, 242090118578155);
    protected override dynamic SolvePart1(string inputFile)
    {
        var stones = Input.ToString(inputFile)
            .SplitToLongs()
            .ToList();

        return CalculateStones(stones, 25);
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var stones = Input.ToString(inputFile)
            .SplitToLongs()
            .ToList();

        return CalculateStones(stones, 75);
    }

    private static long CalculateStones(List<long> stones, int blinks)
    {
        var count = stones.GroupBy(x => x)
            .ToDictionary(group => group.Key, group => (long)group.Count());

        for (var i = 0; i < blinks; i++)
        {
            Dictionary<long, long> nextCount = [];

            foreach (var entry in count)
            {
                var nextStones = TransformStone(entry.Key);

                foreach (var stone in nextStones)
                {
                    if (nextCount.ContainsKey(stone))
                    {
                        nextCount[stone] += entry.Value;
                    }
                    else
                    {
                        nextCount[stone] = entry.Value;
                    }
                }
            }

            count = nextCount;
        }

        return count.Values.Sum();
    }

    private static List<long> TransformStone(long stone)
    {
        if (stone == 0)
        {
            return [1];
        }

        var stoneStr = stone.ToString();

        if (stoneStr.Length % 2 == 0)
        {
            var middleIndex = stoneStr.Length / 2;
            return
            [
                stoneStr[..middleIndex].ToLong(),
                stoneStr[middleIndex..].ToLong()
            ];
        }

        return [stone * 2024];
    }
}