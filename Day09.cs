using System.Transactions;
using AoCHelper;

namespace AOC_2023;

public class Day09 : BaseDay
{
    private readonly string _input;
    private List<List<long>> OASIS_data = new();
    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (string line in _input.Split('\n'))
        {
            OASIS_data.Add(line
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList()
                );
        }
    }

    public override ValueTask<string> Solve_1()
    {
        long sum = 0;
        foreach (var reading in OASIS_data)
        {
            List<List<long>> interpolation = new() { reading };

            while (!interpolation[^1].All(x => x == 0))
            {
                List<long> nextInterpolation = new();
                for (int i = 1; i < interpolation[^1].Count; i++)
                {
                    nextInterpolation.Add(interpolation[^1][i] - interpolation[^1][i - 1]);
                }
                interpolation.Add(nextInterpolation);
            }

            interpolation.Reverse();
            long extrapolation = 0;
            foreach (var row in interpolation)
            {
                extrapolation += row[^1];
                row.Add(extrapolation);
            }

            sum += interpolation[^1][^1];

            // foreach (var row in interpolation)
            // {
            //     Console.WriteLine(string.Join(" ", row));
            // }
        }

        return new(sum.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        foreach (var reading in OASIS_data)
        {
            List<List<long>> interpolation = new() { reading };

            while (!interpolation[^1].All(x => x == 0))
            {
                List<long> nextInterpolation = new();
                for (int i = 1; i < interpolation[^1].Count; i++)
                {
                    nextInterpolation.Add(interpolation[^1][i] - interpolation[^1][i - 1]);
                }
                interpolation.Add(nextInterpolation);
            }

            interpolation.Reverse();
            long extrapolation = 0;
            foreach (var item in interpolation)
            {
                extrapolation = item[0] - extrapolation;
                item.Insert(0, extrapolation);
            }

            sum += interpolation[^1][0];

            // foreach (var row in interpolation)
            // {
            //     Console.WriteLine(string.Join(" ", row));
            // }
        }

        return new(sum.ToString());
    }
}