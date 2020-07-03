using System;

public class SearchData
{
    public Point start;
    public Point end;
    public UnityEngine.YieldInstruction interval;
}

public class AstarData: SearchData
{
    public bool isIgnoreCorner;
    public Action<Point> cmpltCllBck;
}
