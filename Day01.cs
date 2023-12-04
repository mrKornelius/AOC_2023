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
            var (fst, lst) = FindFirstAndLastDigit(line);
            sum += 10 * fst + lst;
        }
        return new(sum.ToString());
    }

    private static (int, int) FindFirstAndLastDigit(string line)
    {
        int fst = line.First(char.IsDigit) - '0';
        int lst = line.Last(char.IsDigit) - '0';
        return (fst, lst);
    }

    public override ValueTask<string> Solve_2()
    {
        int sum = 0;
        var digits = new Dictionary<string, string>() {{"one","o1e"},
                                                       {"two", "t2o"},
                                                       {"three","thr3e"},
                                                       {"four","fo4r"},
                                                       {"five","fi5e"},
                                                       {"six","s6x"},
                                                       {"seven","sev7n"},
                                                       {"eight","e8ght"},
                                                       {"nine","ni9e"},
                                                      };
        List<string> lines = new(_input.Split('\n'));
        for (int i = 0; i < lines.Count; ++i)
        {
            foreach (string digit in digits.Keys)
            {
                lines[i] = lines[i].Replace(digit, digits[digit]);
            }
            var (fst, lst) = FindFirstAndLastDigit(lines[i]);
            sum += 10 * fst + lst;

        }
        return new(sum.ToString());
    }
}