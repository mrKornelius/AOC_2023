using AoCHelper;

namespace AOC_2023;

public class Day04 : BaseDay
{
    private readonly string _input;
    private Dictionary<int, List<int>> winners = new();
    private Dictionary<int, List<int>> tickets = new();

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (var (line, row) in _input.Split('\n').Select((v, i) => (v, i)))
        {
            string[] nums = line.Split(":")[1].Split("|");
            winners[row] = SplitAndParseString(nums[0]);
            tickets[row] = SplitAndParseString(nums[1]);
        }
    }

    private List<int> SplitAndParseString(string nums) => nums
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

    private int CountWinnersInTicket(int game) => tickets[game].Intersect(winners[game]).Count();
    public override ValueTask<string> Solve_1()
    {
        long points = 0;
        for (int game = 0; game < tickets.Count; ++game)
        {
            int correct = CountWinnersInTicket(game);
            // sum += (long)Math.Pow(2, correct - 1);
            points += correct > 0 ? 1 << (correct - 1) : 0;
        }

        return new(points.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        // start with one of each scratchcard
        List<int> scratchCards = Enumerable.Repeat(1, tickets.Count).ToList();

        for (int game = 0; game < tickets.Count; ++game)
        {
            int correct = CountWinnersInTicket(game);
            while (correct > 0)
            {
                scratchCards[game + correct] += scratchCards[game];
                --correct;
            }
        }

        return new(scratchCards.Sum().ToString());
    }
}