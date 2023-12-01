using System.IO.Compression;
using AoCHelper;

namespace AOC_2023;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
        // _input = "onetwothreefourfivesixseveneightnine34";
        // _input = @"two2nine
        // eightwothree
        // abcone2threexyz
        // xtwone3four
        // 4nineeightseven2
        // zoneight234
        // 7pqrstsixteen";
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = 0;
        foreach (string s in _input.Split('\n'))
        {
            var strs = s.Where(char.IsDigit).ToArray();
            var nums = strs.Select(x => int.Parse(x.ToString())).ToArray();
            // Console.WriteLine(s + " " + string.Join(", ", nums) + $" -> {nums[0] * 10 + nums[^1]}");
            sum += 10 * nums[0] + nums[^1];
        }
        return new(sum.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        int sum = 0;

        var digits = new List<string>(new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" });


        foreach (string s in _input.Split("\n"))
        {
            string f = s;
            int i = 0;
            while (i < f.Length)
            {
                foreach (string digit in digits)
                {
                    if (f[i..].StartsWith(digit))
                    {
                        // f = f.Replace(digit, (digits.IndexOf(digit) + 1).ToString());
                        f = f[..i] + (digits.IndexOf(digit) + 1) + f[(i + digit.Length - 1)..];
                        break;
                    }
                }
                i++;
            }

            var strs = f.Where(char.IsDigit).ToArray();
            var nums = strs.Select(x => int.Parse(x.ToString())).ToArray();
            // Console.WriteLine(s + " " + f + " " + string.Join(", ", nums) + $" -> {nums[0] * 10 + nums[^1]}");
            sum += 10 * nums[0] + nums[^1];
        }
        return new(sum.ToString());
    }
}

// 54788 ++