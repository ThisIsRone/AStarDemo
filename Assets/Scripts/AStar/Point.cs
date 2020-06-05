using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Point ParentPoint { get; set; }

    public int F { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x,int y)
    {
        this.X = x;
        this.Y = y;
    }

    public void CalcF()
    {
        this.F = this.G + this.H;
    }
    public override string ToString()
    {
        return string.Format("F:{0} G:{1} H:{2} ( X:{3} Y:{4}) ", F, G, H, X, Y);
    }

    public void PrintPath()
    {
        Debug.LogError("Print path:");
        var parent = ParentPoint;
        while (parent != null)
        {
            Debug.LogError(parent.ToString());
            parent = parent.ParentPoint;
        }
    }
}
