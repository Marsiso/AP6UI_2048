using System.Collections.Concurrent;
using System.Text.Json;
using AP6UI_2048_Solver;
using AP6UI_2048_Solver.Entities;

// Notify user
Console.Clear();
Console.WriteLine("Running ...");

// Measurement
const int runs = 300;

var statisticsCollection = new ConcurrentBag<Statistics>();
Parallel.For(0, runs, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (index) =>
{
    var g2048 = new G2048();
    var statistics = index switch
    {
        < 100 => g2048.ArtificialIntelligenceSolver(runs: 100, moves: 1000),
        < 200 => g2048.ArtificialIntelligenceSolver(runs: 250, moves: 1000),
        _ => g2048.ArtificialIntelligenceSolver(runs: 500, moves: 1000)
    };

    statisticsCollection.Add(statistics);
});

// Single game statistics
var json = JsonSerializer.Serialize(statisticsCollection);
Console.Clear();
Console.WriteLine(json);
Console.WriteLine();

// Overall statistics per number of runs
var distinctRuns = statisticsCollection.Select(statistic => statistic.Runs).Distinct();
var globalStatistics = distinctRuns.Select(distinctRun => GlobalStatistics.ToGlobalStatistics(statisticsCollection.Where(statistic => statistic.Runs == distinctRun).ToList())).ToList();

json = JsonSerializer.Serialize(globalStatistics);
Console.WriteLine(json);
Console.WriteLine();

Console.WriteLine("Press any key to exit ...");
Console.ReadKey();