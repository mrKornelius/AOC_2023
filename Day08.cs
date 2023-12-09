using AoCHelper;

namespace AOC_2023;

public class Day08 : BaseDay
{
    private readonly string _input;
    private string dir;
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
                string[] ll = line.Split(" = ");
                var dest = ll[1].Split(", ");
                maze[ll[0]] = (dest[0][1..], dest[1][..^1]);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int i = 0;
        string current = "AAA";
        string goal = "ZZZ";

        while (current != goal)
        {
            if (dir[i % dir.Length] == 'L')
            {
                current = maze[current].L;
            }
            else
            {
                current = maze[current].R;
            }
            i++;
        }
        return new(i.ToString());
    }
    public override ValueTask<string> Solve_2() => throw new NotImplementedException();
}