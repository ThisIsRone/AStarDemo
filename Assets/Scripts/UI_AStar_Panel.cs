using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AStar_Panel : MonoBehaviour
{
    private Map map;
    private Finder finder;
    private Transform mapRoot;

    private GameObject modlePointer;

    private void Awake()
    {
        map = new Map();
        map.Init();
        finder = new Finder(map);
        mapRoot = transform.Find("MapRoot");
        modlePointer = transform.Find("Pointer").gameObject;
    }

    private void Start()
    {
        CreateMap(map);
        var point = finder.FindPath();
        point.PrintPath();
    }

    private void OnDestroy()
    {
        map.Release();
    }

    public void CreateMap(Map map)
    {
        var mapData = map.mapData;
        int w = mapData.GetLength(0);
        int h = mapData.GetLength(1);
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                int op = mapData[i,j];
                createSigleObj(map, op, i, j);
            }
        }
    }

    private void createSigleObj(Map map, int op, int i, int j)
    {
        GameObject go = GameObject.Instantiate(modlePointer);
        go.SetActive(true);
        go.transform.SetParent(mapRoot);
        var pointer = go.GetComponent<Pointer>();
        pointer?.SetPointer(map, i, j);
    }
}
