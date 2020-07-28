using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseSearch
{
    public BaseSearch(Map map)
    {
        this.map = map;
    }

    /// <summary>
    /// 地图数据
    /// </summary>
    public Map map { get; private set; }

    /// <summary>
    /// 搜寻格子的回调
    /// </summary>
    public Action<Point> SearchCallBack { get; set; }

    /// <summary>
    /// 设置OpenList,以及重新计算G值时的回调
    /// </summary>
    public Action<Point> PointCallBack { get; set; }

    public abstract Point FindPath(SearchData searchData);

    public abstract IEnumerator AsynFindPath(SearchData searchData);
}
