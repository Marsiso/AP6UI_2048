using AP6UI_2048_Solver.Entities;
using AP6UI_2048_Solver.Enums;

namespace AP6UI_2048_Solver;

public sealed class G2048
{
    private int Score { get; set; }
    private int[,] Board { get; } = new int[4, 4];
    private readonly Random _random = new();
    private readonly Statistics _statistics = new();
    private static readonly MoveDirection[] Directions = { MoveDirection.Left, MoveDirection.Up, MoveDirection.Right, MoveDirection.Down };

    public Statistics RandomSolver(int runs = 100, int moves = 1000)
    {
        _statistics.Runs = runs;
        _statistics.MovesLimit = moves;
        
        var hasUpdated = true;
        do
        {
            if (hasUpdated) this.AddTile();
            if (this.IsValid() is false) break;
            var index = this._random.Next(0, 4);
            hasUpdated = this.UpdateBoard(Directions[index]);
            switch (Directions[index])
            {
                case MoveDirection.Left:
                    _statistics.Moves.Left += 1;
                    break;
                case MoveDirection.Up:
                    _statistics.Moves.Up += 1;
                    break;
                case MoveDirection.Right:
                    _statistics.Moves.Right += 1;
                    break;
                case MoveDirection.Down:
                    _statistics.Moves.Down += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        while (true);

        _statistics.Score = Score;
        
        var array = Board.Cast<int>().ToArray();
        _statistics.Tiles.Min = array.Min();
        _statistics.Tiles.Max = array.Max();
        
        return _statistics;
    }

    public Statistics ArtificialIntelligenceSolver(int runs = 100, int moves = 1000)
    {
        _statistics.Runs = runs;
        _statistics.MovesLimit = moves;
        
        var hasUpdated = true;
        do
        {
            if (hasUpdated) this.AddTile();
            if (this.IsValid() is false) break;
            
            var scoresPerMove = new Dictionary<MoveDirection, int>
            {
                { MoveDirection.Left, 0 },
                { MoveDirection.Up, 0 },
                { MoveDirection.Right, 0 },
                { MoveDirection.Down, 0 }
            };
            
            foreach (var direction in Directions)
            {
                var initialMoveBoard = (int[,])this.Board.Clone();
                
                if (UpdateBoard(initialMoveBoard, direction, out var initialScore))
                {
                    AddTile(ref initialMoveBoard, _random);
                    scoresPerMove[direction] += initialScore;
                }
                else
                {
                    continue;
                }

                for (var run = 0; run < runs; run++)
                {
                    var move = 0;
                    var searchBoard = (int[,])initialMoveBoard.Clone();
                    while (IsValid(searchBoard) && move < moves)
                    {
                        var index = _random.Next(0, 4);
                        if (UpdateBoard(searchBoard, Directions[index], out var searchScore))
                        {
                            AddTile(ref searchBoard, _random);
                            scoresPerMove[direction] += searchScore;
                            move++;
                        }
                    }
                }
            }
            
            var bestMove = scoresPerMove.Aggregate((left, right) => left.Value > right.Value ? left : right).Key;
            hasUpdated = this.UpdateBoard(bestMove);
            switch (bestMove)
            {
                case MoveDirection.Left:
                    _statistics.Moves.Left += 1;
                    break;
                case MoveDirection.Up:
                    _statistics.Moves.Up += 1;
                    break;
                case MoveDirection.Right:
                    _statistics.Moves.Right += 1;
                    break;
                case MoveDirection.Down:
                    _statistics.Moves.Down += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        while (true);

        _statistics.Score = Score;
        
        var array = Board.Cast<int>().ToArray();
        _statistics.Tiles.Min = array.Min();
        _statistics.Tiles.Max = array.Max();
        
        return _statistics;
    }

    private static bool UpdateBoard(int[,] board, MoveDirection moveDirection, out int score)
    {
        score = 0;
        var hasUpdated = false;

        var isAlongRow = moveDirection is MoveDirection.Left or MoveDirection.Right;
        var isIncreasing = moveDirection is MoveDirection.Left or MoveDirection.Up;
        
        var innerStart = isIncreasing ? 0 : 3;
        var innerEnd = isIncreasing  ? 3 : 0;

        var drop = isIncreasing 
            ? innerIndex => innerIndex - 1 
            : new Func<int, int>(innerIndex => innerIndex + 1);

        var reverseDrop = isIncreasing 
            ? innerIndex => innerIndex + 1 
            : new Func<int, int>(innerIndex => innerIndex - 1);

        Func<int[,], int, int, int> getTileValue = isAlongRow
            ? (x, i, j) => x[i, j]
            : new Func<int[,], int, int, int>((x, i, j) => x[j, i]);

        Action<int[,], int, int, int> setTileValue = isAlongRow
            ? (x, i, j, v) => x[i, j] = v
            : new Action<int[,], int, int, int>((x, i, j, v) => x[j, i] = v);

        bool InnerCondition(int index) => Math.Min(innerStart, innerEnd) <= index && index <= Math.Max(innerStart, innerEnd);

        for (var row = 0; row < 4; row++)
            for (var col = innerStart; InnerCondition(col); col = reverseDrop(col))
            {
                if (getTileValue(board, row, col) == 0)
                    continue;

                var newCol = col;
                do newCol = drop(newCol);
                while (InnerCondition(newCol) && getTileValue(board, row, newCol) == 0);

                if (InnerCondition(newCol) && getTileValue(board, row, newCol) == getTileValue(board, row, col))
                {
                    var newValue = getTileValue(board, row, newCol) * 2;
                    setTileValue(board, row, newCol, newValue);
                    setTileValue(board, row, col, 0);

                    hasUpdated = true;
                    score += newValue;
                }
                else
                {
                    newCol = reverseDrop(newCol);
                    if (newCol != col) hasUpdated = true;

                    var value = getTileValue(board, row, col);
                    setTileValue(board, row, col, 0);
                    setTileValue(board, row, newCol, value);
                }
            }

        return hasUpdated;
    }

    private bool UpdateBoard(MoveDirection moveDirection)
    {
        var isUpdated = UpdateBoard(this.Board, moveDirection, out var score);
        this.Score += score;
        return isUpdated;
    }

    private bool IsValid() => (from direction in Directions let clone = (int[,])this.Board.Clone() where UpdateBoard(clone, direction, out _) select direction).Any();
    private static bool IsValid(int[,] board) => (from direction in Directions let clone = (int[,])board.Clone() where UpdateBoard(clone, direction, out _) select direction).Any();

    private void AddTile()
    {
        var emptyTiles = new List<Tuple<int, int>>();
        for (var row = 0; row < 4; row++)
            for (var col = 0; col < 4; col++)
                if (Board[row, col] == 0)
                    emptyTiles.Add(new Tuple<int, int>(row, col));

        var index = _random.Next(0, emptyTiles.Count); 
        var tileValue = _random.Next(0, 100) < 95 ? 2 : 4; 
        
        Board[emptyTiles[index].Item1, emptyTiles[index].Item2] = tileValue;
    }
    
    private static void AddTile(ref int[,] board, Random random)
    {
        var emptyTiles = new List<Tuple<int, int>>();
        for (var row = 0; row < 4; row++)
        for (var col = 0; col < 4; col++)
            if (board[row, col] == 0)
                emptyTiles.Add(new Tuple<int, int>(row, col));

        var index = random.Next(0, emptyTiles.Count); 
        var tileValue = random.Next(0, 100) < 95 ? 2 : 4; 
        
        board[emptyTiles[index].Item1, emptyTiles[index].Item2] = tileValue;
    }
}