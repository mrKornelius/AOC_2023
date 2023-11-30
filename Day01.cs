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
        return new(":D");
    }
    public override ValueTask<string> Solve_2() => throw new NotImplementedException();
}