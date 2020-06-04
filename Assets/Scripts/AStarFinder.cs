using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarFinder
{
    /// <summary>
    /// 相邻对角的移动价值
    /// </summary>
    public const int VALU_STEP_CORNER = 14;

    /// <summary>
    /// 相邻直线的移动价值
    /// </summary>
    public const int VALUE_STEP_LINE = 10;

    /// <summary>
    /// 地图数据
    /// </summary>
    public Map map { get; private set; }

    private int[,] MapData;

    private List<Point> CloseList = new List<Point>();

    private List<Point> OpenList = new List<Point>();

    public AStarFinder(Map map)
    {
        this.map = map;
        CatchMapData();
    }

    /// <summary>
    /// 更新最新的地图数据
    /// </summary>
    public void CatchMapData()
    {
        MapData = map.GetMapData();
    }

    public Point FindPath(Point start,Point end,bool isIgnoreCorner)
    {
        OpenList.Add(start);
        while(OpenList.Count != 0)
        {
            //找出F值最小的点
            var tempPoint = OpenList.MinPoint();
            OpenList.RemoveAt(0);
            CloseList.Add(tempPoint);
            var alivePoints = SearchAroundAlivePoint(tempPoint, isIgnoreCorner);
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
                }
            }
            if (OpenList.Get(end) != null)
                return OpenList.Get(end);
        }
        return OpenList.Get(end);
    }


    /// <summary>
    /// 获取9宫格内可到达的点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="IsIgnoreCorner"></param>
    /// <returns></returns>
    public List<Point> SearchAroundAlivePoint(Point point, bool IsIgnoreCorner)
    {
        var points = new List<Point>(9);

        for (int x = point.X - 1; x <= point.X + 1; x++)
            for (int y = point.Y - 1; y <= point.Y + 1; y++)
            {
                if (CanReach(point, x, y, IsIgnoreCorner))
                    points.Add(x, y);
            }
        return points;
    }

    //在二维数组对应的位置不为障碍物
    private bool CanReach(int x, int y)
    {
        return MapData[x, y] == 0;
    }

    public bool CanReach(Point start, int x, int y, bool IsIgnoreCorner)
    {
        if (!CanReach(x, y) || CloseList.Exists(x, y))
            return false;
        else
        {
            if (Math.Abs(x - start.X) + Math.Abs(y - start.Y) == 1)
                return true;
            //如果是斜方向移动, 判断是否 "拌脚"
            else
            {
                if (CanReach(Math.Abs(x - 1), y) && CanReach(x, Math.Abs(y - 1)))
                    return true;
                else
                    return IsIgnoreCorner;
            }
        }
    }

    private void FoundPoint(Point tempStart, Point point)
    {
        var G = CalcG(tempStart, point);
        if (G < point.G)
        {
            point.ParentPoint = tempStart;
            point.G = G;
            point.CalcF();
        }
    }

    private void NotFoundPoint(Point tempStart, Point end, Point point)
    {
        point.ParentPoint = tempStart;
        point.G = CalcG(tempStart, point);
        point.H = CalcH(end, point);
        point.CalcF();
        OpenList.Add(point);
    }

    /// <summary>
    /// 计算G值 只适用于相邻的两个点
    /// </summary>
    /// <param name="start">起始点</param>
    /// <param name="point">目标点</param>
    /// <returns></returns>
    private int CalcG(Point start, Point point)
    {
        int G = (Math.Abs(point.X - start.X) + Math.Abs(point.Y - start.Y)) == 2 ? VALUE_STEP_LINE : VALU_STEP_CORNER;
        int parentG = point.ParentPoint != null ? point.ParentPoint.G : 0;
        return G + parentG;
    }

    /// <summary>
    /// 计算H值
    /// </summary>
    /// <param name="end"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private int CalcH(Point end, Point point)
    {
        int step = Math.Abs(point.X - end.X) + Math.Abs(point.Y - end.Y);
        return step * VALUE_STEP_LINE;
    }

}
