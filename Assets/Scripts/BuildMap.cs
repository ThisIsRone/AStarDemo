using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMap
{
    private Transform mapRoot;

    private GameObject modle;

    private GameObject panel;

    private float panelOffset = 0.1f;

    private int mask;

    public void SetMapRoot(Transform mapRoot)
    {
        this.mapRoot = mapRoot;
    }

    public void Init()
    {
        modle = GameObject.Find("Cube");
        panel = GameObject.Find("Plane");
        mask = LayerMask.NameToLayer("Cube");
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //从摄像机发出到点击坐标的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, mask))
            {
                var pointer = hitInfo.collider.GetComponent<Pointer>();
                pointer?.Switch();
            }
        }
    }

    public void Release()
    {
    }

    public void CreateMap(Map map)
    {
        var mapData = map.mapData;
        int w = mapData.GetLength(0);
        int h = mapData.GetLength(1);
        panel.transform.localScale = new Vector3(w * panelOffset,1,h * panelOffset);
        panel.transform.position = new Vector3((w - 1) * 0.5f, -0.5f, (h -1) * 0.5f);
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                int op = mapData[i, j];
                createSigleObj(map,op, i, j);
            }
        }
    }

    private void createSigleObj(Map map,int op,int i,int j)
    {
        GameObject go = GameObject.Instantiate(modle);
        go.transform.SetParent(mapRoot);
        var pointer = go.GetComponent<Pointer>();
        pointer?.SetPointer(map, i, j);
    }
}
