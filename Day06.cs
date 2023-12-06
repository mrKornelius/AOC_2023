using AoCHelper;
using Spectre.Console;

namespace AOC_2023;

public class Day06 : BaseDay
{
    private readonly string _input;
    private List<int> times = new();
    private List<int> distances = new();
    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (string line in _input.Split('\n'))
        {
            if (line.Contains("Time"))
            {
                times = ExtractNumbers(line);
            }
            if (line.Contains("Distance"))
            {
                distances = ExtractNumbers(line);
            }
        }

        // Console.WriteLine(string.Join(", ", times));
        // Console.WriteLine(string.Join(", ", distances));
    }

    private List<int> ExtractNumbers(string str)
    {
        return str
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        long ans = 1;
        foreach ((int t, int d) in times.Zip(distances))
        {
            ans *= CalculateWinningPositions(t, d);
        }
        return new(ans.ToString());
    }

    private static long CalculateWinningPositions(long t, long d)
    {
        double mid = t / 2.0;
        double x = Math.Sqrt(mid * mid - (d + 1)) + mid;
        double x_min = Math.Floor(x);
        int low = (int)((x_min * (t - x_min) > d) ? x_min : Math.Ceiling(x));
        Console.WriteLine($"mid {mid} \nx {x} \nx_min {x_min} \nlow {low}");

        long res = 2 * Math.Abs(t / 2 - low) + (t % 2 == 0 ? 1 : 0);
        Console.WriteLine(res);
        Console.WriteLine();
        return res;
    }

    public override ValueTask<string> Solve_2()
    {
        string tt = "", dd = "";
        foreach (int t in times)
        {
            tt += t.ToString();
        }
        foreach (int d in distances)
        {
            dd += d.ToString();
        }


        return new(CalculateWinningPositions(long.Parse(tt), long.Parse(dd)).ToString());
    }
}