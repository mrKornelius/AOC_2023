using System.Text;
using AoCHelper;

namespace AOC_2023;

public class Day19 : BaseDay
{
    private readonly string _input;
    private readonly Dictionary<string, List<Rule>> workflows = new();
    private readonly List<Dictionary<char, int>> parts = new();

    public Day19()
    {
        _input = File.ReadAllText(InputFilePath);
        bool flag = true;
        foreach (string line in _input.Split('\n'))
        {
            if (line == "") flag = false;
            else if (flag)
            {
                var p1 = line.Split('{');
                var rules = p1[1][..^1].Split(',');

                workflows[p1[0]] = new();
                foreach (var r in rules)
                {
                    if (r.Contains(':'))
                    {
                        char cat = r[0];
                        char comp = r[1];
                        var pp = r[2..].Split(':');
                        int lim = int.Parse(pp[0]);
                        string dest = pp[1];

                        workflows[p1[0]].Add(new DecisionRule(cat, comp, lim, dest));
                    }
                    else
                    {
                        workflows[p1[0]].Add(new Rule(r));
                    }
                }
            }
            else
            {
                var rr = line[1..^1].Split(',').Select(x => int.Parse(x[2..])).ToArray();
                string xmas = "xmas";
                Dictionary<char, int> tt = new();
                for (int i = 0; i < 4; ++i)
                {
                    tt[xmas[i]] = rr[i];
                }
                parts.Add(tt);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        // foreach (var k in workflows.Keys)
        // {
        //     Console.WriteLine($"{k}: {string.Join(", ", workflows[k])}");
        // }
        // Console.WriteLine();

        // foreach (var part in parts)
        // {
        //     Console.WriteLine($"{string.Join(", ", part)}");
        // }
        // Console.WriteLine();

        long acceptedParts = 0;
        foreach (var part in parts)
        {
            // Console.WriteLine($"{string.Join(", ", part)}");
            string current = "in";
            while (current != "A" && current != "R")
            {
                List<Rule> rules = workflows[current];
                foreach (Rule r in rules)
                {
                    // Console.Write(r);
                    current = r.MakeDecision(part);
                    // Console.WriteLine($"  -> {current}");
                    if (current != "") break;
                }
            }

            // Console.WriteLine();
            if (current == "A")
            {
                acceptedParts += part.Values.Sum();
            }
        }

        return new(acceptedParts.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        Dictionary<char, int[]> defaultRange = new()
        {
            ['x'] = new[] { 1, 4000 },
            ['m'] = new[] { 1, 4000 },
            ['a'] = new[] { 1, 4000 },
            ['s'] = new[] { 1, 4000 }
        };
        Node root = new("in", defaultRange);
        ConnectTree(root);
        // Console.WriteLine(root);
        return new(CalculateAcceptedCombinations(root).ToString());
    }

    private long CalculateAcceptedCombinations(Node root)
    {
        if (root.Name == "A") return root.Combinations();
        long sum = 0;
        foreach (Node n in root.Children)
        {
            sum += CalculateAcceptedCombinations(n);
        }
        return sum;
    }
    private void ConnectTree(Node current)
    {
        if (current.Name == "A" || current.Name == "R") { return; }
        // making a copy of the part ranges so we can modify them...
        var xmas = current.Xmas.ToDictionary(e => e.Key, e => (int[])e.Value.Clone());

        foreach (Rule r in workflows[current.Name])
        {
            if (r is DecisionRule dr)
            {
                if (dr.Comparer == '<')
                {
                    var lo = xmas[dr.Category][0];
                    var hi = xmas[dr.Category][1];
                    xmas[dr.Category][1] = dr.Limit - lo;
                    current.Children.Add(new(dr.Destination, xmas));
                    xmas[dr.Category][0] = dr.Limit;
                    xmas[dr.Category][1] = hi - xmas[dr.Category][1];
                }
                else
                {
                    var lo = xmas[dr.Category][0];
                    var hi = xmas[dr.Category][1];
                    xmas[dr.Category][0] = dr.Limit + 1;
                    xmas[dr.Category][1] = hi - dr.Limit + lo - 1;
                    current.Children.Add(new(dr.Destination, xmas));
                    xmas[dr.Category][0] = lo;
                    xmas[dr.Category][1] = hi - xmas[dr.Category][1];
                }
            }
            else
            {
                current.Children.Add(new(r.Destination, xmas));
            }
        }

        foreach (Node child in current.Children)
        {
            ConnectTree(child);
        }
    }
}

class Node
{
    public string Name { get; set; }
    public Dictionary<char, int[]> Xmas { get; set; }
    public Rule Rule { get; set; } = new("?");
    public List<Node> Children { get; set; } = new();

    public Node(string name, Dictionary<char, int[]> xmas)
    {
        Name = name;
        Xmas = xmas.ToDictionary(e => e.Key, e => (int[])e.Value.Clone());
    }

    public long Combinations()
    {
        long sum = 1;
        foreach (var v in Xmas.Values)
        {
            sum *= v[1];
        }
        return sum;
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        // sb.AppendLine($"{Name}: {Combinations()}");
        sb.AppendLine($"{Name}: {string.Join(", ", Xmas.Values.Select(x => $"({x[0]}, {x[1]})").ToList())}");
        sb.AppendLine(Rule.ToString());
        foreach (Node c in Children)
        {
            sb.AppendLine($"  {c}");
        }
        return sb.ToString();
    }

}
class Rule
{
    public string Destination { get; set; }
    public Rule(string destination) => Destination = destination;
    public virtual string MakeDecision(Dictionary<char, int> part) => Destination;
    public override string ToString() => $" ==> {Destination}";
}
class DecisionRule : Rule
{
    public char Category { get; set; }
    public char Comparer { get; set; }
    public int Limit { get; set; }

    // public static readonly Dictionary<char, int> categoryToIndex = new() { ['x'] = 0, ['m'] = 1, ['a'] = 2, ['s'] = 3 };
    public DecisionRule(char category, char comparer, int limit, string destination) : base(destination)
    {
        Category = category;
        Comparer = comparer;
        Limit = limit;
    }

    public override string MakeDecision(Dictionary<char, int> part)
    {
        if (Comparer == '<')
        {
            return part[Category] < Limit ? Destination : "";
        }
        else
        {
            return part[Category] > Limit ? Destination : "";
        }
    }

    public override string ToString()
    {
        return $"{Category} {Comparer} {Limit} => {Destination}";
    }

}