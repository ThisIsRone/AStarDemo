using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : BaseSearch
{
    /// <summary>
    /// 相邻对角的移动价值
    /// </summary>
    public const int VALU_STEP_CORNER = 14;

    /// <summary>
    /// 相邻直线的移动价值
    /// </summary>
    public const int VALUE_STEP_LINE = 10;

    protected List<Point> CloseList = new List<Point>();

    protected List<Point> OpenList = new List<Point>();

    public AStarSearch(Map map) : base(map)
    {

    }

    public override Point FindPath(SearchData searchData)
    {
        if (!(searchData is AstarData))
        {
            Debug.LogError("数据类型错误！");
            return null;
        }
        AstarData astarData = searchData as AstarData;
        Point start = astarData.start;
        Point end = astarData.end;
        bool isIgnoreCorner = astarData.isIgnoreCorner;
        OpenList.Add(start);
        while (OpenList.Count != 0)
        {
            stepSearch(start, end, isIgnoreCorner);
            if (OpenList.Get(end) != null)
                return OpenList.Get(end);
        }
        return OpenList.Get(end);
    }

    public override IEnumerator AsynFindPath(SearchData searchData)
    {
        if (!(searchData is AstarData))
        {
            Debug.LogError("数据类型错误！");
            yield break;
        }
        AstarData astarData = searchData as AstarData;
        Point start = astarData.start;
        Point end = astarData.end;
        bool isIgnoreCorner = astarData.isIgnoreCorner;
        Action<Point> cmpltCllBck = astarData.cmpltCllBck;
        OpenList.Add(start);
        while (OpenList.Count != 0)
        {
            stepSearch(start, end, isIgnoreCorner);
            if (OpenList.Get(end) != null)
                break;
            yield return searchData.interval;
        }
        Point result = OpenList.Get(end);
        cmpltCllBck?.Invoke(result);
    }

    private void stepSearch(Point start, Point end, bool isIgnoreCorner)
    {
        //找出F值最小的点
        var tempPoint = OpenList.PopMinPoint();
        //OpenList.RemoveAt(0);
        CloseList.Add(tempPoint);
        var alivePoints = GetGridAlivePoint(tempPoint, isIgnoreCorner);
        for (int i = 0; i < alivePoints.Count; i++)
        {
            Point p = alivePoints[i];
            if (OpenList.Exists(p))
            {
                //计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
                FoundPoint(tempPoint, p);        
            }
            else
            {
                //如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
                NotFoundPoint(tempPoint, end, p);
                p.PrintPath();
            }
        }
    }

    /// <summary>
    /// 获取9宫格内可到达的点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="IsIgnoreCorner"></param>
    /// <returns></returns>
    public List<Point> GetGridAlivePoint(Point point, bool IsIgnoreCorner)
    {
        var points = new List<Point>(9);
        for (int x = point.X - 1; x <= point.X + 1; x++)
            for (int y = point.Y - 1; y <= point.Y + 1; y++)
            {
                if (IsWalkable(point, x, y, IsIgnoreCorner))
                {
                    points.Add(x, y);
                }

            }
        return points;
    }

    //在二维数组对应的位置不为障碍物
    protected bool IsNotObstacle(int x, int y)
    {
        if (!IsVaildPoint(x, y))
        {
            return false;
        }
        return map.GetValue(x, y) == 0;
    }

    //是否是地图上的点
    protected bool IsVaildPoint(int x, int y)
    {
        int h = map.mapData.GetLength(0);
        int w = map.mapData.GetLength(1);
        if (x < 0 || x >= w)
        {
            return false;
        }
        if (y < 0 || y >= h)
        {
            return false;
        }
        return true;
    }

    public bool IsWalkable(Point start, int x, int y, bool IsIgnoreCorner)
    {
        //障碍点
        if (!IsNotObstacle(x, y))
        {
            return false;
        }
        //已经处理过的点
        if (CloseList.Exists(x, y))
        {
            return false;
        }
        //直线方向
        if (Math.Abs(x - start.X) + Math.Abs(y - start.Y) == 1)
        {
            return true;
        }
        //斜向方向
        bool p1Walkable = IsNotObstacle(start.X, y);
        bool p2Walkable = IsNotObstacle(x, start.Y);
        if (p1Walkable && p2Walkable)
        {
            return true;
        }
        else
        {
            return IsIgnoreCorner;
        }
    }

    protected void FoundPoint(Point tempStart, Point point)
    {
        var G = CalcG(tempStart, point);
        if (G < point.G)
        {
            point.ParentPoint = tempStart;
            //point.G = G;
            point.F = point.H + G;
        }
    }

    protected void NotFoundPoint(Point tempStart, Point end, Point point)
    {
        point.ParentPoint = tempStart;
        point.G = CalcG(tempStart, point);
        point.H = CalcH(end, point);
        point.CalcF();
        OpenList.Add(point);
        CallBack?.Invoke(point);
    }

    /// <summary>
    /// 计算G值 只适用于相邻的两个点
    /// </summary>
    /// <param name="start">起始点</param>
    /// <param name="point">目标点</param>
    /// <returns></returns>
    protected int CalcG(Point start, Point point)
    {
        int G = (Math.Abs(point.X - start.X) + Math.Abs(point.Y - start.Y)) == 2 ? VALU_STEP_CORNER:VALUE_STEP_LINE;
        int parentG = point.ParentPoint != null ? point.ParentPoint.G : 0;
        return G + parentG;
    }

    /// <summary>
    /// 计算H值
    /// </summary>
    /// <param name="end"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    protected int CalcH(Point end, Point point)
    {
        int step = Math.Abs(point.X - end.X) + Math.Abs(point.Y - end.Y);
        return step * VALUE_STEP_LINE;
    }

}
