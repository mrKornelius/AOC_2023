using System.Text;
using AoCHelper;

namespace AOC_2023;

public class Day14 : BaseDay
{
    private readonly string _input;
    private List<char[]> G = new();

    public Day14()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (var line in _input.Split('\n'))
        {
            G.Add(line.ToCharArray());
        }
    }

    public override ValueTask<string> Solve_1()
    {
        TiltNorth();
        return new(CalculateTotalLoad(G).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int ROUNDS = 1000000000;
        TiltWest();
        TiltSouth();
        TiltEast();

        Dictionary<int, int> hash2number = new();
        Dictionary<int, string> number2grid = new();
        string finalGrid = "";

        for (int i = 1; i < 1000; i++)
        {
            string s = GetGridAsString();
            int h = s.GetHashCode();
            // Console.Write($"{i,4}: {h,15} - {CalculateTotalLoad(G)}");
            if (!hash2number.ContainsKey(h))
            {
                hash2number[h] = i;
                number2grid[i] = s;
            }
            else
            {
                int old = hash2number[h];
                int m = i - old;
                // Console.WriteLine();
                // Console.WriteLine($"old={old} - m={m} - i={i}");
                // Console.WriteLine($" rep={(ROUNDS - old) % m}");
                finalGrid = number2grid[old + (ROUNDS - old) % m];
                break;
            }
            // Console.WriteLine();
            RunCycle();
        }
        // Console.WriteLine();
        return new(CalculateTotalLoad(finalGrid).ToString());
    }

    private string GetGridAsString()
    {
        StringBuilder sb = new();
        foreach (var row in G)
        {
            sb.Append(new string(row));
        }
        return sb.ToString();
    }
    private long CalculateTotalLoad(string s)
    {
        int len = (int)Math.Sqrt(s.Length);
        return CalculateTotalLoad(new List<char[]>(s.Chunk(len)));
    }
    private static long CalculateTotalLoad(List<char[]> G)
    {
        long sum = 0;
        for (int row = 0; row < G.Count; row++)
        {
            for (int col = 0; col < G[row].Length; col++)
            {
                if (G[row][col] == 'O')
                {
                    sum += G.Count - row;
                }
            }
        }
        return sum;
    }
    private void RunCycle()
    {
        TiltNorth();
        TiltWest();
        TiltSouth();
        TiltEast();
    }
    private void TiltNorth()
    {
        for (int row = 0; row < G.Count; row++)
        {
            for (int col = 0; col < G[row].Length; col++)
            {
                if (G[row][col] == 'O')
                {
                    int r = row - 1;
                    while (r >= 0 && G[r][col] == '.') { r--; }
                    r++;
                    if (r != row)
                    {
                        G[row][col] = '.';
                        G[r][col] = 'O';
                    }
                }
            }
        }
    }
    private void TiltSouth()
    {
        for (int row = G.Count - 1; row >= 0; row--)
        {
            for (int col = 0; col < G[row].Length; col++)
            {
                if (G[row][col] == 'O')
                {
                    int r = row + 1;
                    while (r < G.Count && G[r][col] == '.') { r++; }
                    r--;
                    if (r != row)
                    {
                        G[row][col] = '.';
                        G[r][col] = 'O';
                    }
                }
            }
        }
    }
    private void TiltWest()
    {
        for (int col = 0; col < G[0].Length; col++)
        {
            for (int row = 0; row < G.Count; row++)
            {
                if (G[row][col] == 'O')
                {
                    int c = col - 1;
                    while (c >= 0 && G[row][c] == '.') { c--; }
                    c++;
                    if (c != col)
                    {
                        G[row][col] = '.';
                        G[row][c] = 'O';
                    }
                }
            }
        }
    }
    private void TiltEast()
    {
        for (int col = G[0].Length - 1; col >= 0; col--)
        {
            for (int row = 0; row < G.Count; row++)
            {
                if (G[row][col] == 'O')
                {
                    int c = col + 1;
                    while (c < G[0].Length && G[row][c] == '.') { c++; }
                    c--;
                    if (c != col)
                    {
                        G[row][col] = '.';
                        G[row][c] = 'O';
                    }
                }
            }
        }
    }
}