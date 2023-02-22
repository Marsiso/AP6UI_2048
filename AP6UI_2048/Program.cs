using System.Collections.Concurrent;
using System.Text.Json;
using AP6UI_2048_Solver;
using AP6UI_2048_Solver.Entities;

// Notify user
Console.Clear();
Console.WriteLine("Running");

// Measurement
const int runs = 150;

var statisticsCollection = new ConcurrentBag<Statistics>();
Parallel.For(0, runs, (index) =>
{
    var g2048 = new G2048();

    Statistics? statistics;
    switch (index)
    {
        case < 50:
            Console.Clear();
            Console.WriteLine("Running .");
            statistics = g2048.ArtificialIntelligenceSolver(runs: 100, moves: 10000);
            break;
        case < 100:
            Console.Clear();
            Console.WriteLine("Running ..");
            statistics = g2048.ArtificialIntelligenceSolver(runs: 500, moves: 10000);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Running ...");
            statistics = g2048.ArtificialIntelligenceSolver(runs: 1000, moves: 10000);
            break;
    }

    statisticsCollection.Add(statistics);
});

// Notify user
Console.Clear();
Console.WriteLine("Processing statistics");

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