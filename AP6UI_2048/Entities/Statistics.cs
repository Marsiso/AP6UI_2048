using AP6UI_2048_Solver.Enums;

namespace AP6UI_2048_Solver.Entities;

public sealed class Statistics
{
    public int Score { get; set; }
    public int MovesLimit { get; set; }
    public int Runs { get; set; }
    public Moves Moves { get; set; } = new();
    public Tiles Tiles { get; set; } = new();
    
    public static Statistics operator +(Statistics op1, Statistics op2)
    {
        op1.Score += op2.Score;
        op1.Runs = op1.Runs == op2.Runs
            ? op1.Runs
            : throw new Exception($"Operator add exception. Runs must be equal {op1.Runs} != {op2.Runs}");
        op1.MovesLimit = op1.MovesLimit == op2.MovesLimit
            ? op1.MovesLimit
            : throw new Exception(
                $"Operator add exception. Moves must be equal {op1.MovesLimit} != {op2.MovesLimit}");
        op1.Moves += op2.Moves;
        op1.Tiles += op2.Tiles;
        
        return op1;
    }

    public static Statistics operator /(Statistics op1, int count)
    {
        op1.Score = Convert.ToInt32(Math.Round(op1.Score / (double)count));
        op1.Moves /= count;
        op1.Tiles /= count;
        
        return op1;
    }
}