using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AStar_Panel : MonoBehaviour
{
    private Map map;
    private Finder finder;
    private Transform mapRoot;
    private GridLayoutGroup  grid;

    private GameObject modlePointer;

    private void Awake()
    {
        map = new Map();
        map.Init();
        finder = new Finder(map);
        mapRoot = transform.Find("Scroll View/Viewport/Content");
        modlePointer = transform.Find("Pointer").gameObject;
        grid = mapRoot.GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        CreateMap(map);
        //var point = finder.FindPath();
        //point.PrintPath();
    }

    private void OnDestroy()
    {
        map.Release();
    }

    public void CreateMap(Map map)
    {
        var mapData = map.mapData;
        int h = mapData.GetLength(0);
        int w = mapData.GetLength(1);
        grid.constraintCount = w;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                createSigleObj(map, x, y);
            }
        }
    }

    private void createSigleObj(Map map, int x, int y)
    {
        GameObject go = GameObject.Instantiate(modlePointer);
        go.SetActive(true);
        go.transform.SetParent(mapRoot,false);
        var pointer = go.GetComponent<Pointer>();
        pointer?.SetPointer(map, x, y);
    }
}
