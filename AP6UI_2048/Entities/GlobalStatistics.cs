using AP6UI_2048_Solver.Enums;

namespace AP6UI_2048_Solver.Entities;

public sealed class GlobalStatistics
{
    public int MovesLimit { get; set; }
    public int Runs { get; set; }
    public Score Score { get; set; } = new();
    public Tiles Tiles { get; set; } = new();
    public AverageTiles AverageTiles { get; set; } = new();
    public AverageMoves AverageMoves { get; set; } = new();

    public static GlobalStatistics ToGlobalStatistics(ICollection<Statistics> statistics)
    {
        var average  = statistics.Aggregate((current, statistic) => current + statistic) / statistics.Count;
        var globalStatistics = new GlobalStatistics
        {
            Runs = average.Runs,
            MovesLimit = average.MovesLimit,
            Score = new Score
            {
                Wins = statistics.Count(statistic => statistic.Tiles.Max >= 2048),
                Loses = statistics.Count(statistic => statistic.Tiles.Max < 2048),
                Average = average.Score,
                Min = statistics.Min(statistic => statistic.Score), 
                Max = statistics.Max(statistic => statistic.Score)
            },
            Tiles = new Tiles
            {
                Min = statistics.Min(statistic => statistic.Tiles.Min), 
                Max = statistics.Max(statistic => statistic.Tiles.Max)
            },
            AverageTiles = new AverageTiles
            {
                Min = average.Tiles.Min, 
                Max = average.Tiles.Max
            },
            AverageMoves = new AverageMoves
            {
                Left = average.Moves.Left,
                Up = average.Moves.Up,
                Right = average.Moves.Right,
                Down = average.Moves.Down
            }
        };

        return globalStatistics;
    }
}