using AoCHelper;

namespace AOC_2023;

public class Day02 : BaseDay
{
    private readonly string _input;
    private Dictionary<int, List<(int r, int g, int b)>> games = new();

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (string line in _input.Split('\n'))
        {
            var lineParts = line.Split(": ");
            int game = int.Parse(lineParts[0].Split(" ")[1]);

            List<(int r, int g, int b)> drawList = new();
            foreach (var draw in lineParts[1].Split("; "))
            {
                (int r, int g, int b) cubes = (0, 0, 0);
                foreach (var drawnCubes in draw.Split(", "))
                {
                    var drawParts = drawnCubes.Split(" ");
                    int num = int.Parse(drawParts[0]);
                    switch (drawParts[1].Trim())
                    {
                        case "red": cubes.r = num; break;
                        case "green": cubes.g = num; break;
                        case "blue": cubes.b = num; break;
                    }
                }
                drawList.Add(cubes);
            }
            games[game] = drawList;
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = 0;
        (int r, int g, int b) maxCubes = (12, 13, 14);

        foreach (int game in games.Keys)
        {
            bool gameOK = true;
            foreach (var (r, g, b) in games[game])
            {
                if (r > maxCubes.r || g > maxCubes.g || b > maxCubes.b)
                {
                    gameOK = false;
                    break;
                }
            }
            if (gameOK) sum += game;
        }

        return new(sum.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        foreach (int game in games.Keys)
        {
            (int r, int g, int b) minPossible = (0, 0, 0);
            foreach (var (r, g, b) in games[game])
            {
                minPossible.r = Math.Max(minPossible.r, r);
                minPossible.g = Math.Max(minPossible.g, g);
                minPossible.b = Math.Max(minPossible.b, b);
            }
            sum += minPossible.r * minPossible.g * minPossible.b;
        }

        return new(sum.ToString());
    }
}