using System;

public class SearchData
{
    public Point start;
    public Point end;
    public UnityEngine.YieldInstruction interval;
    public bool isIgnoreCorner;

}

public class AstarData: SearchData
{
    public Action<Point> cmpltCllBck;
}
