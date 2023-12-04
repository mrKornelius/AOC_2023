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
        int row = 0;
        foreach (string line in _input.Split('\n'))
        {
            var nums = line.Split(": ")[1].Trim().Split(" | ");
            winners[row] = nums[0].Replace("  ", " ").Split().Select(int.Parse).ToList();
            tickets[row] = nums[1].Trim().Replace("  ", " ").Split().Select(int.Parse).ToList();
            row++;
        }
    }

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
        List<long> scratchCards = new();
        // start ith one of each scratchcard
        foreach (var _ in tickets) scratchCards.Add(1);

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

    private int CountWinnersInTicket(int game)
    {
        int correct = 0;
        foreach (int n in tickets[game])
        {
            if (winners[game].Contains(n)) ++correct;
        }

        return correct;
    }
}