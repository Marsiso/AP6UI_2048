namespace AP6UI_2048_Solver.Entities;

public sealed class Moves
{
    public int Left { get; set; }
    public int Up { get; set; }
    public int Right { get; set; }
    public int Down { get; set; }
    
    public int Total => Left + Up + Right + Down;
}