using AoCHelper;

namespace AOC_2023;

public class Day15 : BaseDay
{
    private readonly string _input;
    private Dictionary<int, List<(string label, int f)>> boxes = new();

    public Day15()
    {
        _input = File.ReadAllText(InputFilePath);

    }

    public override ValueTask<string> Solve_1()
    {
        long sum = 0;
        foreach (string step in _input.Split(","))
        {
            sum += Hash(step);
            // Console.WriteLine($"{step,-5}: {Hash(step),4}");
        }
        return new(sum.ToString());
    }

    private int Hash(string s)
    {
        int hash = 0;
        foreach (char c in s)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }
        return hash;
    }

    public override ValueTask<string> Solve_2()
    {
        foreach (string step in _input.Split(','))
        {
            if (step.Contains('-'))
            {
                string label = step.Replace("-", "");
                int h = Hash(label);
                if (boxes.ContainsKey(h))
                {
                    int i = boxes[h].FindIndex(x => x.label == label);
                    if (i >= 0)
                    {
                        boxes[h].RemoveAt(i);
                    }
                }
            }
            else
            {
                var parts = step.Split('=');
                string label = parts[0];
                int focalLength = int.Parse(parts[1]);
                int h = Hash(label);

                if (boxes.ContainsKey(h))
                {
                    int i = boxes[h].FindIndex(x => x.label == label);
                    if (i >= 0)
                    {
                        var lens = boxes[h][i];
                        lens.f = focalLength;
                        boxes[h][i] = lens; // TODO: eeehh?!
                    }
                    else
                    {
                        boxes[h].Add((label, focalLength));
                    }
                }
                else
                {
                    boxes[h] = new()
                    {
                        (label, focalLength)
                    };
                }

            }
        }

        long sum = 0;
        foreach (var box in boxes)
        {
            // Console.WriteLine($"Box {box.Key,3}: {string.Join(" ", box.Value)}");
            for (int i = 0; i < box.Value.Count; ++i)
            {
                sum += (box.Key + 1) * (i + 1) * box.Value[i].f;
            }
        }
        return new(sum.ToString());
    }
}