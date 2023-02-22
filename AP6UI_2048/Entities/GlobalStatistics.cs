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
        var globalStatistics = new GlobalStatistics
        {
            Runs = statistics.First().Runs,
            MovesLimit = statistics.First().MovesLimit,
            Score = new Score
            {
                Wins = statistics.Count(statistic => statistic.Tiles.Max >= 2048),
                Loses = statistics.Count(statistic => statistic.Tiles.Max < 2048),
                Average = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Score))),
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
                Min = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Tiles.Min))), 
                Max = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Tiles.Max)))
            },
            AverageMoves = new AverageMoves
            {
                Left = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Moves.Left))),
                Up = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Moves.Up))),
                Right = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Moves.Right))),
                Down = Convert.ToInt32(Math.Round(statistics.Average(statistic => statistic.Moves.Down)))
            }
        };
        

        return globalStatistics;
    }
}