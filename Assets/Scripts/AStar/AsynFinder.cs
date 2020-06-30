using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsynFinder : Finder
{

    public AsynFinder(Map map):base(map)
    {

    }


    private IEnumerable conroutine = null;
    private WaitForSeconds waitHalfSeconds = new WaitForSeconds(0.5f); 

    public IEnumerator AsynFindPath(Point start, Point end, bool isIgnoreCorner, Action<Point> cmpltCllBck)
    {
        OpenList.Add(start);
        while (OpenList.Count != 0)
        {
            //找出F值最小的点
            var tempPoint = OpenList.MinPoint();
            OpenList.RemoveAt(0);
            CloseList.Add(tempPoint);
            var alivePoints = SearchAroundAlivePoint(tempPoint, false);
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
                break;
            yield return waitHalfSeconds;
        }
        Point result = OpenList.Get(end);
        cmpltCllBck?.Invoke(result);
    }
}
