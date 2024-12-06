using Utils;

namespace Year2024.Day05;

// https://adventofcode.com/2024/day/5

public class Day05 : BaseDay
{
    protected override Answers Part1Answers => new(143, 5948);
    protected override Answers Part2Answers => new(123, 3062);
    protected override int SolvePart1(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var rules = GetRules(lines);
        var updates = GetUpdates(lines);

        return CalculateMiddleValue(GetValidUpdates(updates, rules));
    }

    protected override int SolvePart2(string inputFile)
    {
        var lines = Input.ToLines(inputFile);

        var rules = GetRules(lines);
        var updates = GetUpdates(lines);

        var incorrectUpdates = updates.Except(GetValidUpdates(updates, rules));
        ReorderUpdates(incorrectUpdates, rules);

        return CalculateMiddleValue(incorrectUpdates);
    }

    private static int CalculateMiddleValue(IEnumerable<int[]> updates)
    {
        var result = 0;
        foreach (var update in updates)
        {
            var middleIndex = update.Length / 2;
            var middle = update[middleIndex];
            result += middle;
        }
        return result;
    }

    private static List<int[]> GetValidUpdates(IEnumerable<int[]> updates, Dictionary<int, List<int>> rules)
    {
        var validUpdates = new List<int[]>();

        foreach (var update in updates)
        {
            if (ValidateUpdate(update, rules).IsValid)
            {
                validUpdates.Add(update);
            }
        }

        return validUpdates;
    }

    private static void ReorderUpdates(IEnumerable<int[]> updates, Dictionary<int, List<int>> rules)
    {
        foreach (var update in updates)
        {
            var validationResult = ValidateUpdate(update, rules);

            while (!validationResult.IsValid)
            {
                var indexOfUpdate = validationResult.IndexOfUpdate!.Value;
                var indexOfRule = validationResult.IndexOfRule!.Value;
                
                (update[indexOfUpdate], update[indexOfRule]) = (update[indexOfRule], update[indexOfUpdate]);
                
                validationResult = ValidateUpdate(update, rules);
            }
        }
    }

    private static VerificationResult ValidateUpdate(int[] update, Dictionary<int, List<int>> rules)
    {
        foreach (var updatePage in update)
        {
            if (!rules.TryGetValue(updatePage, out var ruleList))
            {
                continue;
            }
            foreach (var rule in ruleList)
            {
                var indexOfUpdate = Array.IndexOf(update, updatePage);
                var indexOfRule = Array.IndexOf(update, rule);
                indexOfRule = indexOfRule == -1 ? int.MaxValue : indexOfRule;
                if (indexOfUpdate > indexOfRule)
                {
                    return new VerificationResult(false, indexOfUpdate, indexOfRule);
                }
            }
        }
        return new VerificationResult(true);
    }

    private static Dictionary<int, List<int>> GetRules(string[] lines)
    {
        var rules = new Dictionary<int, List<int>>();

        foreach (var ruleString in lines.TakeWhile(s => s != string.Empty))
        {
            var parts = ruleString.Split("|");
            var pageNumber = parts[0].ToInt();
            var rule = parts[1].ToInt();

            if (!rules.ContainsKey(pageNumber))
            {
                rules[pageNumber] = [];
            }

            rules[pageNumber].Add(rule);
        }

        return rules;
    }

    private static List<int[]> GetUpdates(string[] lines)
    {
        var updates = lines.SkipWhile(s => s != string.Empty)
            .Skip(1)
            .Select(x => x.SplitToIntegers(',')).ToList();
        return updates;
    }

    internal record VerificationResult(bool IsValid, int? IndexOfUpdate = null, int? IndexOfRule = null);
}