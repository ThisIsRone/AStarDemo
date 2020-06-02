using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMap
{
    int[,] map_array = null;

    public Vector2 size { get;private set; }

    public float length { get;private set; }


    public void Init()
    {
        map_array = {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                           { 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1},
                           { 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
                           { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1},
                           { 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 1},
                           { 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1},
                           { 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
                           { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
    }

    public void Release()
    {

    }

    /// <summary>
    /// 获取地图数据
    /// </summary>
    /// <returns></returns>
    public int[,] GetMap()
    {
        return map_array;
    }

    /// <summary>
    /// 设置地图的大小
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetSize(int x,int y)
    {
        size = new Vector2(x, y);
    }

    /// <summary>
    /// 设置格子的长度
    /// </summary>
    /// <param name="length"></param>
    public void SetLength(float length)
    {
        this.length = length;
    }
}
