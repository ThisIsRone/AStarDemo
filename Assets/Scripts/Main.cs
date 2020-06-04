using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private Map map;
    private BuildMap buildMap;

    private void Start()
    {
        map = new Map();
        map.Init();
        buildMap = new BuildMap();
        buildMap.Init();
        GameObject mapRoot = GameObject.Find("MapRoot");
        buildMap.SetMapRoot(mapRoot.transform);
        buildMap.CreateMap(map);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        map.Release();
        buildMap.Release();
    }
}
