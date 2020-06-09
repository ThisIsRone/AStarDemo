using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public int[,] mapData { get; private set; }

    public Vector2 size { get; private set; }

    public float length { get; private set; }


    public void Release()
    {
        mapData = null;
    }

    public void RebuildMap(int width,int height)
    {
        mapData = new int[height, width];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int op = Random.Range(0, 9) > 7 ? 1 : 0;
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
}
