namespace AP6UI_2048_Solver.Entities;

public sealed class Tiles
{
    public int Min { get; set; }
    public int Max { get; set; }
    
    public static Tiles operator +(Tiles op1, Tiles op2)
    {
        op1.Min += op2.Min;
        op1.Max += op2.Max;

        return op1;
    }

    public static Tiles operator /(Tiles op1, int count)
    {
        op1.Min = Convert.ToInt32(Math.Round(op1.Min / (double)count));
        op1.Max = Convert.ToInt32(Math.Round(op1.Max / (double)count));

        return op1;
    }
}