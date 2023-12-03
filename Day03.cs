using System.Data;
using System.Data.Common;
using AoCHelper;

namespace AOC_2023;

public class Day03 : BaseDay
{
    private readonly string _input;
    List<string> G = new();
    private readonly int R, C;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (string line in _input.Split('\n'))
        {
            G.Add(line);
        }
        R = G.Count;
        C = G[0].Length;
    }

    // My variabel convention:
    //  G  - grid
    //  R  - total rows
    //  r  - this row
    //  rr - temp row
    //  dr - change in row
    //  nr - new row (nr = r+dr) 

    public override ValueTask<string> Solve_1()
    {
        long sum = 0;
        List<(int, int)> symbolCoords = new();
        for (int r = 0; r < R; ++r)
        {
            for (int c = 0; c < C; ++c)
            {
                if (!char.IsDigit(G[r], c) && G[r][c] != '.')
                {
                    symbolCoords.Add((r, c));
                }
            }
        }

        // List<(int, int)> symbolCoords = FindCoordsOfSymbols("!#%&/()=?@-+*_"); //TODO: fix this...
        foreach (var coord in symbolCoords)
        {
            List<long> nums = ExtractNumbers(FindCoordsOfDigitsSurroundingCoord(coord));
            sum += nums.Sum();
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        foreach (var coord in FindCoordsOfSymbols("*"))
        {
            List<long> nums = ExtractNumbers(FindCoordsOfDigitsSurroundingCoord(coord));
            sum += (nums.Count == 2) ? nums[0] * nums[1] : 0;
        }
        return new(sum.ToString());
    }

    // Finds a characters that from a given string of symbols and returns a list of
    // coordinates.
    public List<(int, int)> FindCoordsOfSymbols(string symbols)
    {
        List<(int, int)> coords = new();
        for (int r = 0; r < R; ++r)
        {
            for (int c = 0; c < C; ++c)
            {
                if (symbols.Contains(G[r][c]))
                {
                    coords.Add((r, c));
                }
            }
        }
        return coords;
    }

    // Extract all digits relative coordinates to (r,c) in a list.
    private List<(int, int)> FindCoordsOfDigitsSurroundingCoord((int r, int c) coord)
    {
        List<(int, int)> digits = new();
        for (int dr = -1; dr < 2; ++dr)
        {
            for (int dc = -1; dc < 2; ++dc)
            {
                var (nr, nc) = (coord.r + dr, coord.c + dc);
                if (nr < 0 || nr >= R || nc < 0 || nc >= C) continue;
                if (char.IsDigit(G[nr][nc])) digits.Add((nr, nc));
            }
        }
        return digits;
    }

    // Until list is empty find the numbers that correspond to each digit in the list and
    // delete all coordinate of that number in the list.
    // When the list is empty we return a list of the found numbers. 
    private List<long> ExtractNumbers(List<(int, int)> digits)
    {
        List<long> nums = new();
        while (digits.Count > 0)
        {
            (int r, int c) = digits[0];
            // creating a left and a right pointer to surround the number
            int lc = c, rc = c;
            while (lc >= 0 && char.IsDigit(G[r][lc])) { lc--; }
            while (rc < R && char.IsDigit(G[r][rc])) { rc++; }

            // back up left pointer since it should point to first digit in the number, 
            // the right pointer should point to the index after the last digit of the number.
            lc++;
            nums.Add(long.Parse(G[r][lc..rc]));

            for (int i = lc; i < rc; i++)
            {
                digits.Remove((r, i));
            }
        }
        return nums;
    }
}
