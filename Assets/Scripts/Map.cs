using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    int[,] map_array = null;

    public Vector2 size { get; private set; }

    public float length { get; private set; }


    public void Init()
    {
        map_array = GetMapData();
    }

    public void Release()
    {
        map_array = null;
    }

    /// <summary>
    /// 获取地图数据
    /// </summary>
    /// <returns></returns>
    public int[,] GetMapData()
    {
        int[,] map_data = new int[,]{
                           { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                           { 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1},
                           { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                           { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1},
                           { 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1},
                           { 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1},
                           { 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                           { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        return map_data;
    }
}
