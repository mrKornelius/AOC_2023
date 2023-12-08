using AoCHelper;

namespace AOC_2023;

public class Day07 : BaseDay
{
    private readonly string _input;
    private List<(string cards, int bid)> hands = new();

    public Day07()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (string line in _input.Split('\n'))
        {
            string cards = line.Split(' ')[0].Trim();
            int bid = int.Parse(line.Split(' ')[1].Trim());
            hands.Add((cards, bid));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        hands.Sort((a, b) => CompareTo(a.cards, b.cards));

        long winnings = 0;
        for (int rank = 0; rank < hands.Count; ++rank)
        {
            winnings += (rank + 1) * hands[rank].bid;
        }

        return new(winnings.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        hands.Sort((a, b) => CompareTo_Jokers(a.cards, b.cards));

        long winnings = 0;
        for (int rank = 0; rank < hands.Count; ++rank)
        {
            winnings += (rank + 1) * hands[rank].bid;
        }

        return new(winnings.ToString());
    }

    public int CompareTo(string a, string b)
    {
        int sa = HandStrength(a);
        int sb = HandStrength(b);
        if (sa > sb) return 1;
        else if (sa < sb) return -1;
        else
        {
            foreach (var (ca, cb) in a.Zip(b))
            {
                if (ca != cb) return CompareCards(ca, cb);
            }
            return 0;
        }
    }

    private int CompareCards(char a, char b)
    {
        if (a == b) return 0;
        if (GetCardValue(a) > GetCardValue(b)) return 1;
        else return -1;
    }

    private int GetCardValue(char a) => a switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        _ => int.Parse(a.ToString())
    };

    public static int HandStrength(string hand)
    {
        return int.Parse(
            string.Join("", hand
                .GroupBy(x => x)
                .Select(x => x.Count())
                .OrderBy(x => -x)
                )
            .PadRight(5, '0')
            );
    }

    public int CompareTo_Jokers(string a, string b)
    {
        int ja = a.Count(c => c == 'J');
        int jb = b.Count(c => c == 'J');

        string na = a.Replace("J", "");
        string nb = b.Replace("J", "");

        int sa = HandStrength(na);
        int sb = HandStrength(nb);

        sa += 10000 * ja;
        sb += 10000 * jb;

        if (sa > sb) return 1;
        else if (sa < sb) return -1;
        else
        {
            foreach (var (ca, cb) in a.Zip(b))
            {
                if (ca != cb) return CompareCards_Jokers(ca, cb);
            }
            return 0;
        }
    }

    private int CompareCards_Jokers(char a, char b)
    {
        if (a == b) return 0;
        if (GetCardValue_Jokers(a) > GetCardValue_Jokers(b)) return 1;
        else return -1;
    }

    private int GetCardValue_Jokers(char a) => a switch
    {
        'J' => 1,
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'T' => 10,
        _ => int.Parse(a.ToString())
    };
}