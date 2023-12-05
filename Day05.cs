using System.Runtime.InteropServices;
using AoCHelper;

namespace AOC_2023;

public class Day05 : BaseDay
{
    private readonly string _input;
    private readonly List<long> seeds = new();
    private readonly Dictionary<int, List<List<long>>> maps = new();
    public Day05()
    {
        _input = File.ReadAllText(InputFilePath);

        int mapType = 0;
        foreach (string line in _input.Split('\n'))
        {
            if (line.Contains("seeds:"))
            {
                seeds = line
                    .Split(":")[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(long.Parse)
                    .ToList();
            }
            else if (line.Contains("map"))
            {
                // next map
                ++mapType;
                maps[mapType] = new();
            }
            else if (line.Trim() == "")
            {
                continue;
            }
            else
            {
                maps[mapType].Add(
                    line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(long.Parse)
                    .ToList()
                );
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        // foreach (var m in maps.Keys)
        // {
        //     Console.WriteLine(m);
        //     foreach (var lst in maps[m])
        //     {
        //         Console.WriteLine($" {string.Join(' ', lst)}");
        //     }
        // }

        long minLocation = long.MaxValue;
        foreach (long seed in seeds)
        {
            long val = seed;
            Console.WriteLine($" --- Seed: {seed} ---");
            for (int map = 1; map <= maps.Count; ++map)
            {
                bool flag = false;
                foreach (var mapping in maps[map])
                {
                    if (CheckMapping(mapping, val, out long result))
                    {
                        val = result;
                        Console.WriteLine($"mapping {string.Join(", ", mapping)} \t-> {val}");
                        flag = true;
                        break;
                    }
                }
                if (!flag) Console.WriteLine($" No mapping: {val} \t-> {val}");
            }

            Console.WriteLine($"Location: {val}");
            minLocation = Math.Min(minLocation, val);


        }

        return new(minLocation.ToString());
    }


    private bool CheckMapping(List<long> mapping, long val, out long result)
    {
        long dest = mapping[0];
        long source = mapping[1];
        long length = mapping[2];
        if (val >= source && val < source + length)
        {
            result = val + dest - source;
            return true;
        }
        result = 0;
        return false;
    }

    public override ValueTask<string> Solve_2()
    {

        return new("");
    }
}