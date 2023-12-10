using AoCHelper;

namespace AOC_2023;

public class Day08 : BaseDay
{
    private readonly string _input;
    private readonly string dir = "";
    private Dictionary<string, (string L, string R)> maze = new();

    public Day08()
    {
        bool flag = true;
        _input = File.ReadAllText(InputFilePath);
        foreach (string line in _input.Split('\n'))
        {
            if (flag)
            {
                dir = line.Trim();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            else
            {
                string[] parts = line.Split(" = ");
                var dest = parts[1].Replace("(", "").Replace(")", "").Split(", ");
                maze[parts[0]] = (dest[0], dest[1]);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int i = 0;
        string current = "AAA";

        while (current != "ZZZ")
        {
            current = dir[i % dir.Length] == 'L' ? maze[current].L : maze[current].R;
            i++;
        }
        return new(i.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<long> cycles = new();
        int i = 0;

        foreach (string pos in maze.Keys)
        {
            if (!pos.EndsWith('A')) continue;

            string current = pos;
            i = 0;
            while (!current.EndsWith("Z"))
            {
                current = dir[i % dir.Length] == 'L' ? maze[current].L : maze[current].R;
                i++;
            }
            cycles.Add(i);
            // Console.WriteLine($"{pos} -> {current} in {i} steps");
        }

        return new(CalculateAnswer(cycles).ToString());
    }

    // By using the cycles for each start -> end, when the sequence repeats, we can calculate
    // a value for when all the different sequences will be at their end points simultaneously.
    // 
    // Basically if you have two cycles a=8 and b=12 you can easily find when they will both be
    // their end points by multiplying the cycle lengths i.e. 8*12 = 96. But you can do better
    // because they will actually be at endpoints much earlier. By using the LCM (least common multiple)
    // we will find that point, LCM(a,b) = (a*b)/GCD(a,b), and GCD (greatest common divisor) is 
    // calculated using Euclid's algorithm. GCD(8,12) = 4. Which means that 8*12/4 = 24 is the first 
    // time the cycles cross.
    // 
    public static long CalculateAnswer(List<long> cycles)
    {
        long gcd = cycles.Aggregate(GCD);
        long lcm = cycles.Aggregate((a, b) => (a * b) / gcd);
        return lcm;
    }

    // Euclid's Algorithm
    public static long GCD(long a, long b)
    {
        while (b != 0) (a, b) = (b, a % b);
        return a;
    }
}
