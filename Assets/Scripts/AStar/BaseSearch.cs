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

    public Action<Point> CallBack { get; set; }

    public abstract Point FindPath(SearchData searchData);

    public abstract IEnumerator AsynFindPath(SearchData searchData);
}
