using AP6UI_2048_Solver.Enums;

namespace AP6UI_2048_Solver.Entities;

public sealed class Statistics
{
    public int Score { get; set; }
    public int MovesLimit { get; set; }
    public int Runs { get; set; }
    public Moves Moves { get; set; } = new();
    public Tiles Tiles { get; set; } = new();
}