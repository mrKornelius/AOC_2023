using AoCHelper;

namespace AOC_2023;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = 0;
        foreach (string line in _input.Split('\n'))
        {
            var nums = line.Where(char.IsDigit).Select(c => c - '0').ToArray();
            // Console.WriteLine(s + " " + string.Join(", ", nums) + $" -> {nums[0] * 10 + nums[^1]}");
            sum += 10 * nums[0] + nums[^1];
        }
        return new(sum.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        int sum = 0;
        var digits = new List<string>(new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" });

        foreach (string _line in _input.Split("\n"))
        {
            string line = _line; // not allowed to modify the foreach-variable...
            int i = 0;
            while (i < line.Length)
            {
                foreach (string digit in digits)
                {
                    if (line[i..].StartsWith(digit))
                    {
                        // Apparently we don't consume the characters of the spelled out digits.
                        // This means that "eightwone" -> "8igh2w1ne" and not "8w1" which would
                        // happen if "eight" was consumed into "8" and "one" into "1".
                        // Instead we just change the first character of the spelled out digit
                        // to the digits number.
                        line = line[..i] + (digits.IndexOf(digit) + 1) + line[(i + 1)..];
                        // f = f.Insert(i, (digits.IndexOf(digit) + 1).ToString()).Remove(i + 1, 1);
                        break;
                    }
                }
                i++;
            }

            var nums = line.Where(char.IsDigit).Select(c => c - '0').ToArray();
            // Console.WriteLine(s + " " + f + " " + string.Join(", ", nums) + $" -> {nums[0] * 10 + nums[^1]}");
            sum += 10 * nums[0] + nums[^1];
        }
        return new(sum.ToString());
    }
}