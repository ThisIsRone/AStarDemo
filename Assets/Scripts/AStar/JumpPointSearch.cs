using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointSearch : BaseSearch
{

    private static readonly int MaxJumpPointDistance = 128;

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

    public JumpPointSearch(Map map) : base(map)
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
        var neighbors = GetNeighbors(tempPoint, isIgnoreCorner);
        int count = neighbors.Count;
        for (int i = 0; i < count; i++)
        {
            Point neighbor = neighbors[i];
            var neighborPoint = GetNeighborNode(start, neighbor, end);
            if (OpenList.Exists(neighborPoint))
            {
                //计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
                FoundPoint(tempPoint, neighborPoint);        
            }
            else
            {
                //如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
                NotFoundPoint(tempPoint, end, neighborPoint);
                neighbor.PrintPath();
            }
        }
    }

    /// <summary>
    /// 获取9宫格内可到达的点
    /// </summary>
    /// <param name="point"></param>
    /// <param name="IsIgnoreCorner"></param>
    /// <returns></returns>
    public List<Point> GetNeighbors(Point point, bool IsIgnoreCorner)
    {
        var points = new List<Point>();
        Point parent = point.ParentPoint;
        //if (parentNode == null)
        //{
        //    return base.GetNeighbors(point, ref neighbors);
        //}
        int xDirection = Mathf.Clamp(point.X - parent.X, -1, 1);
        int yDirection = Mathf.Clamp(point.Y - parent.Y, -1, 1);
        if (xDirection != 0 && yDirection != 0)
        {
            //assumes positive direction for variable naming
            bool neighbourUp =IsWalkable(point.X, point.Y + yDirection);
            bool neighbourRight =IsWalkable(point.X + xDirection, point.Y);
            bool neighbourLeft =IsWalkable(point.X - xDirection, point.Y);
            bool neighbourDown =IsWalkable(point.X, point.Y - yDirection);

            if (neighbourUp)
            {
               var p = new Point(point.X, point.Y + yDirection);
                points.Add(p);
            }
            if (neighbourRight)
            {
                var p = new Point(point.X, point.Y + yDirection);
                points.Add(p);
            }
            if ((neighbourUp || neighbourRight) && IsWalkable(point.X + xDirection, point.Y + yDirection))
            {
                var p = new Point(point.X, point.Y + yDirection);
                points.Add(p);
            }
            if (!neighbourLeft && neighbourUp && IsWalkable(point.X - xDirection, point.Y + yDirection))
            {
                var p = new Point(point.X, point.Y + yDirection);
                points.Add(p);
            }
            if (!neighbourDown && neighbourRight && IsWalkable(point.X + xDirection, point.Y - yDirection))
            {
                var p = new Point(point.X, point.Y + yDirection);
                points.Add(p);
            }
        }
        else
        {
            if (xDirection == 0)
            {
                if (IsWalkable(point.X, point.Y + yDirection))
                {
                    var p = new Point(point.X, point.Y + yDirection);
                    points.Add(p);

                    if (!IsWalkable(point.X + 1, point.Y) &&IsWalkable(point.X + 1, point.Y + yDirection))
                    {
                        var p1 = new Point(point.X, point.Y + yDirection);
                        points.Add(p);
                    }
                    if (!IsWalkable(point.X - 1, point.Y) &&IsWalkable(point.X - 1, point.Y + yDirection))
                    {
                        var p1 = new Point(point.X, point.Y + yDirection);
                        points.Add(p);
                    }
                }
            }
            else
            {
                if (IsWalkable(point.X + xDirection, point.Y))
                {
                    var p = new Point(point.X, point.Y + yDirection);
                    points.Add(p);

                    if (!IsWalkable(point.X, point.Y + 1) &&IsWalkable(point.X + xDirection, point.Y + 1))
                    {
                        var p1 = new Point(point.X, point.Y + yDirection);
                        points.Add(p);
                    }
                    if (!IsWalkable(point.X, point.Y - 1) &&IsWalkable(point.X + xDirection, point.Y - 1))
                    {
                        var p2 = new Point(point.X, point.Y + yDirection);
                        points.Add(p);
                    }
                }
            }
        }
        return points;
    }

    private Point GetNeighborNode(Point currentNode, Point neighbor, Point end)
    {
        int xDirection = neighbor.X - currentNode.X;
        int yDirection = neighbor.Y - currentNode.Y;

        var point = Jump(neighbor.X, neighbor.Y, xDirection, yDirection, MaxJumpPointDistance, end);
        return point;
    }

    private Point Jump(int posX, int posY, int xDirection, int yDirection, int depth, Point end)
    {
        if (!IsWalkable(posX, posY))
            return null;
        if (depth == 0 || (end.X == posX && end.Y == posY))
            return new Point(posX, posY);

        if (xDirection != 0 && yDirection != 0)
        {
            if ((!IsWalkable(posX - xDirection, posY) && IsWalkable(posX - xDirection, posY + yDirection)) ||
                (!IsWalkable(posX, posY - yDirection) && IsWalkable(posX + xDirection, posY - yDirection)))
            {
                return new Point(posX, posY);
            }

            if (IsWalkable(posX + xDirection, posY + yDirection) &&
                !IsWalkable(posX + xDirection, posY) &&
                !IsWalkable(posX, posY + yDirection))
            {
                return null;
            }

            if (Jump(posX + xDirection, posY, xDirection, 0, depth - 1, end) != null || Jump(posX, posY + yDirection, 0, yDirection, depth - 1, end) != null)
            {
                return new Point(posX, posY);
            }
        }
        else
        {
            if (xDirection != 0)
            {
                if ((IsWalkable(posX + xDirection, posY + 1) && !IsWalkable(posX, posY + 1)) ||
                    (IsWalkable(posX + xDirection, posY - 1) && !IsWalkable(posX, posY - 1)))
                {
                    return new Point(posX, posY);
                }
            }
            else
            {
                if ((IsWalkable(posX + 1, posY + yDirection) && !IsWalkable(posX + 1, posY)) ||
                    (IsWalkable(posX - 1, posY + yDirection) && !IsWalkable(posX - 1, posY)))
                {
                    return new Point(posX, posY);
                }
            }
        }

        return Jump(posX + xDirection, posY + yDirection, xDirection, yDirection, depth - 1, end);
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

    public bool IsWalkable(int x, int y)
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
        return true;
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
