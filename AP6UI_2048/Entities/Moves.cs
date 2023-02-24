namespace AP6UI_2048_Solver.Entities;

public sealed class Moves
{
    public int Left { get; set; }
    public int Up { get; set; }
    public int Right { get; set; }
    public int Down { get; set; }
    
    public int Total => Left + Up + Right + Down;
    
    public static Moves operator +(Moves op1, Moves op2)
    {
        op1.Left += op2.Left;
        op1.Up += op2.Up;
        op1.Right += op2.Right;
        op1.Down += op2.Down;

        return op1;
    }

    public static Moves operator /(Moves op1, int count)
    {
        op1.Left = Convert.ToInt32(Math.Round(op1.Left / (double)count));
        op1.Up = Convert.ToInt32(Math.Round(op1.Up / (double)count));
        op1.Right = Convert.ToInt32(Math.Round(op1.Right / (double)count));
        op1.Down = Convert.ToInt32(Math.Round(op1.Down / (double)count));
        
        return op1;
    }
}