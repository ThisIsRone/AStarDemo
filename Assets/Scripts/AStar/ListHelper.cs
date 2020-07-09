using System.Collections.Generic;
using System.Linq;

public static class ListExtand
{
    public static bool Exists(this List<Point> points, Point point)
    {
        foreach (Point p in points)
            if ((p.X == point.X) && (p.Y == point.Y))
                return true;
        return false;
    }

    public static bool Exists(this List<Point> points, int x, int y)
    {
        foreach (Point p in points)
            if ((p.X == x) && (p.Y == y))
                return true;
        return false;
    }

    public static Point MinPoint(this List<Point> points)
    {
        points = points.OrderBy(p => p.F).ToList();
        return points[0];
    }

    /// <summary>
    /// 弹出最小的点
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static Point PopMinPoint(this List<Point> points)
    {
        Point t = null;
        if (points.Count > 0)
        {
            int minIndex = 0;
            int f = 0;
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                f = f == 0 ? p.F : f;
                if (p.F < f)
                {
                    f = p.F;
                    minIndex = i;
                }
            }
            t = points[minIndex];
            points.RemoveAt(minIndex);
        }        
        return t;
    }

    public static void Add(this List<Point> points, int x, int y)
    {
        Point point = new Point(x, y);
        points.Add(point);
    }

    public static Point Get(this List<Point> points, Point point)
    {
        foreach (Point p in points)
            if ((p.X == point.X) && (p.Y == point.Y))
                return p;
        return null;
    }

    public static void Remove(this List<Point> points, int x, int y)
    {
        foreach (Point point in points)
        {
            if (point.X == x && point.Y == y)
                points.Remove(point);
        }
    }
}
