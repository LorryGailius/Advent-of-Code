using System.Text;
using Utils;
using Utils.Extensions;

namespace Year2024.Day09;

// https://adventofcode.com/2024/day/9

public class Day09 : BaseDay
{
    protected override Answers Part1Answers => new(1928, 6323641412437);
    protected override Answers Part2Answers => new(2858, 6351801932670);
    protected override dynamic SolvePart1(string inputFile)
    {
        var disk = GetDisk(inputFile);
        CompressDisk(disk);

        return disk.CheckSum();
    }

    protected override dynamic SolvePart2(string inputFile)
    {
        var disk = GetDisk(inputFile);
        CompressDiskBlocks(disk);

        return disk.CheckSum();
    }

    private static Disk GetDisk(string inputFile)
    {
        var line = Input.ToString(inputFile);

        var dataBlocks = new List<DataBlock>();
        var id = 0;
        var isData = true;

        foreach (var character in line)
        {
            var amount = character.ToInt();

            Enumerable.Range(0, amount).ToList().ForEach(_ =>
            {
                dataBlocks.Add(new DataBlock(isData, isData ? id : null));
            });

            id += isData ? 1 : 0;
            isData = !isData;
        }

        return new(dataBlocks);
    }

    private static void CompressDisk(Disk disk)
    {
        var dataBlockIndexes = disk.DataBlocks.Select((dataBlock, index) => (dataBlock, index)).Where(x => x.dataBlock.isData).ToList();

        for (var i = 0; i < disk.DataBlocks.Count; i++)
        {
            if (!dataBlockIndexes.Any())
            {
                break;
            }

            if (!disk.DataBlocks[i].isData)
            {
                var dataIndex = dataBlockIndexes.Last().index;

                (disk.DataBlocks[i], disk.DataBlocks[dataIndex]) = (disk.DataBlocks[dataIndex], disk.DataBlocks[i]);
                dataBlockIndexes.RemoveAt(dataBlockIndexes.Count - 1);
            }
            else
            {
                dataBlockIndexes.RemoveAt(0);
            }
        }
    }

    private static void CompressDiskBlocks(Disk disk)
    {
        var dataBlockGroups = disk.DataBlocks.Select((dataBlock, index) => (dataBlock, index))
            .Where(x => x.dataBlock.isData).GroupBy(x => x.dataBlock.id).ToList();

        List<(List<DataBlock> freeBlocks, int index)> freeSpaceGroups = [];

        for (var i = 0; i < disk.DataBlocks.Count; i++)
        {
            var currentBlock = disk.DataBlocks[i];

            if (!currentBlock.isData)
            {
                var skip = 0;
                List<DataBlock> freeBlocks = [];

                while (!currentBlock.isData)
                {
                    if (i + skip >= disk.DataBlocks.Count)
                    {
                        break;
                    }

                    freeBlocks.Add(currentBlock);

                    skip++;
                    currentBlock = disk.DataBlocks[i + skip];
                }

                if (freeBlocks.Any())
                {
                    freeSpaceGroups.Add((freeBlocks, i));
                }

                i += skip;
            }
        }

        for (int i = dataBlockGroups.Count - 1; i >= 0; i--)
        {
            var dataBlockGroup = dataBlockGroups[i];

            var freeSpaceGroup = freeSpaceGroups.FirstOrDefault(x =>
                x.freeBlocks.Count >= dataBlockGroup.Count() && dataBlockGroup.First().index > x.index);

            if (freeSpaceGroup is (null, 0))
            {
                continue;
            }

            var freeSpaceIndex = freeSpaceGroup.index;
            var dataBlocks = dataBlockGroup.ToList();

            for (var j = 0; j < dataBlocks.Count; j++)
            {
                var dataBlock = dataBlocks[j];
                (disk.DataBlocks[dataBlock.index], disk.DataBlocks[freeSpaceIndex + j]) = (
                    disk.DataBlocks[freeSpaceIndex + j], disk.DataBlocks[dataBlock.index]);
                freeSpaceGroup.index++;
                freeSpaceGroup.freeBlocks?.RemoveAt(0);
            }

            if (!freeSpaceGroup.freeBlocks!.Any())
            {
                freeSpaceGroups.Remove(freeSpaceGroup);
            }
            else
            {
                var indexInGroup = freeSpaceGroups.FindIndex(x => x.index == freeSpaceIndex);
                freeSpaceGroups[indexInGroup] = freeSpaceGroup;
            }
        }
    }

    internal record Disk(List<DataBlock> DataBlocks)
    {
        public override string ToString()
        {
            var disk = new StringBuilder();
            DataBlocks.ForEach(dataBlock =>
            {
                disk.Append(dataBlock.isData ? $"{dataBlock.id}" : ".");
            });
            return disk.ToString();
        }

        public long CheckSum()
        {
            return DataBlocks.Select((dataBlock, index) => dataBlock.isData ? dataBlock.id! * index : 0).Sum(x => (long)x!.Value);
        }
    };

    internal record DataBlock(
        bool isData,
        int? id = null
    );
}