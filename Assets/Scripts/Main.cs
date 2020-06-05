using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private Map map;
    private Finder finder;
    private BuildMap buildMap;
    private void Awake()
    {
        map = new Map();
        map.Init();
        buildMap = new BuildMap();
        buildMap.Init();
        GameObject mapRoot = GameObject.Find("MapRoot");
        buildMap.SetMapRoot(mapRoot.transform);
        finder = new Finder(map);
    }

    private void Start()
    {
        buildMap.CreateMap(map);
        var point = finder.FindPath();
        point.PrintPath();
    }

    // Update is called once per frame
    void Update()
    {
        buildMap.Update();
    }

    private void OnDestroy()
    {
        map.Release();
        buildMap.Release();
    }
}
