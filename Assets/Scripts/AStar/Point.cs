using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    
    /// <summary>
    /// 计算F父节点上的值值
    /// </summary>
    public void CalcPathF()
    {
        int totalF = 0;
        var parent = ParentPoint;
        while (parent != null)
        {
            parent.CalcF();
            totalF += parent.G;
            totalF += parent.H;
            parent = parent.ParentPoint;
        }
        this.F = totalF + this.G + this.H;
    }  
    
    public void PrintPath()
    {
        string msg = "";
        var parent = this;
        while (parent != null)
        {
            msg += string.Format("- ( X:{0} Y:{1})  ", parent. X, parent.Y);
            parent = parent.ParentPoint;

        }
    }
    public override string ToString()
    {
        return string.Format("F:{0} G:{1} H:{2} ( X:{3} Y:{4}) ", F, G, H, X, Y);
    }

    public string ToPoint()
    {
        return string.Format("(x:{0},y:{1})", X, Y);
    }
}
