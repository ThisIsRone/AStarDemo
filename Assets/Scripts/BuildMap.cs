using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMap
{
    private int[,] mapData;

    private Vector2 singleSize = Vector2.one;

    private Transform mapRoot;

    private GameObject modle;

    public void SetMapRoot(Transform mapRoot)
    {
        this.mapRoot = mapRoot;
    }

    public void Init()
    {
        GameObject modle = GameObject.Find("Cube");

    }

    public void Release()
    {
    }

    public void CreateMap(Map map)
    {
        mapData = map.GetMapData();
        int w = mapData.GetLength(0);
        int h = mapData.GetLength(1);
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                int op = mapData[i, j];
                createSigleObj(op, i, j);
            }
        }
    }

    public void SetSingleSize(float x,float y)
    {
        singleSize = new Vector2(x, y);
    }

    private void createSigleObj(int op,int i,int j)
    {
        if (op > 0)
        {
            GameObject go = GameObject.Instantiate(modle);
            go.name = i + "_" + j;
            go.transform.SetParent(mapRoot);
            go.transform.localPosition = new Vector3(i * singleSize.x, 0, j * singleSize.y);
        }
    }
}
