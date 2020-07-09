using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public int[,] mapData { get; private set; }

    public Vector2 size { get; private set; }

    public float length { get; private set; }

    public Vector2 start { get; set; }
    public Vector2 end { get; set; }


    public void Release()
    {
        mapData = null;
    }

    public void RebuildMap(int width,int height)
    {
        start = new Vector2(7, 10);
        end = new Vector2(14, 10);
        mapData = new int[height, width];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int op = 0;//Random.Range(0, 9) > 7 ? 1 : 0;
                mapData[j, i] = op;
            }
        }
    }

    public int GetValue(int x,int y)
    {
        return mapData[y,x];
    }   

    public void SetValue(int x, int y,int value)
    {
        mapData.SetValue(value, y, x);
    }

    public bool IsStart(int x,int y)
    {
        return (int)start.x == x && (int)start.y  == y;
    }

    public bool IsEnd(int x, int y)
    {
        return (int)end.x == x && (int)end.y == y;
    }
}
