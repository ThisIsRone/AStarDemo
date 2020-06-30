using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AStar_Panel : MonoBehaviour
{
    private Map map;
    private Finder finder;
    private AsynFinder asynFinder;
    private Transform mapRoot;
    private GridLayoutGroup grid;
    private GameObject modlePointer;
    [SerializeField]
    private Slider sldWidth = null;
    [SerializeField]
    private Text txtWidth = null;
    [SerializeField]
    private Slider sldHeight = null;
    [SerializeField]
    private Text txtHeigh = null;
    [SerializeField]
    private Button btnBuildMap = null;

    [SerializeField]
    private InputField ipfStartX = null;
    [SerializeField]
    private InputField ipfStartY = null;
    [SerializeField]
    private InputField ipfEndX = null;
    [SerializeField]
    private InputField ipfEndY = null;
    [SerializeField]
    private Button btnFinder = null;

    private Coroutine coroutine = null;

    private void Awake()
    {
        mapRoot = transform.Find("Scroll View/Viewport/Content");
        modlePointer = transform.Find("Pointer").gameObject;
        grid = mapRoot.GetComponent<GridLayoutGroup>();
        sldWidth.onValueChanged.AddListener(sldWidthChange);
        sldHeight.onValueChanged.AddListener(sldHeighChange);
        btnBuildMap.onClick.AddListener(onClickDrawMap);
        btnFinder.onClick.AddListener(onClickAsynFinder);

    }
    private void Start()
    {
        onClickDrawMap();
    }

    private void OnDestroy()
    {
        map.Release();
    }

    private void CleanMap()
    {
        for (int i = 0; i < mapRoot.childCount; i++)
        {
            var child = mapRoot.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
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
        go.transform.SetParent(mapRoot, false);
        var pointer = go.GetComponent<Pointer>();
        pointer?.SetPointer(map, x, y);
    }


    private void sldWidthChange(float v)
    {
        txtWidth.text = Mathf.FloorToInt(v).ToString();
    }

    private void sldHeighChange(float v)
    {
        txtHeigh.text = Mathf.FloorToInt(v).ToString();
    }

    private void onClickDrawMap()
    {
        map = new Map();
        finder = new Finder(map);
        finder.OpenListCallBack = OpenListCallBack;
        asynFinder = new AsynFinder(map);
        asynFinder.OpenListCallBack = OpenListCallBack;
        CleanMap();
        int width = Mathf.FloorToInt(sldWidth.value);
        int height = Mathf.FloorToInt(sldHeight.value);
        
        map.RebuildMap(width, height);
        CreateMap(map);
        //设置输入限制
        ipfStartX.characterLimit = width;
        ipfStartY.characterLimit = height;
        ipfEndX.characterLimit = width;
        ipfEndY.characterLimit = height;

    }

    private void onClickFinder()
    {
        int startX = int.Parse(ipfStartX.text);
        int startY = int.Parse(ipfStartY.text);
        int endX = int.Parse(ipfEndX.text);
        int endY = int.Parse(ipfEndY.text);
        Point start = new Point(startX, startY);
        Point end = new Point(endX, endY);
        var point = GetPoint(start, end);
        var parent = point.ParentPoint;
        while (parent != null)
        {
            string rootName = parent.ToPoint();
            Transform root = mapRoot.transform.Find(rootName);
            Pointer pointer = root?.GetComponent<Pointer>();
            pointer?.SetSelect();
            parent = parent.ParentPoint;
        }
        Transform endRoot = mapRoot.transform.Find(end.ToPoint());
        Pointer endPointer = endRoot?.GetComponent<Pointer>();
        endPointer?.SetSelect();
    }

    private void onClickAsynFinder()
    {
        int startX = int.Parse(ipfStartX.text);
        int startY = int.Parse(ipfStartY.text);
        int endX = int.Parse(ipfEndX.text);
        int endY = int.Parse(ipfEndY.text);
        Point start = new Point(startX, startY);
        Point end = new Point(endX, endY);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(asynFinder.AsynFindPath(start, end, true, (Point point) =>
        {
            var parent = point.ParentPoint;
            while (parent != null)
            {
                string rootName = parent.ToPoint();
                Transform root = mapRoot.transform.Find(rootName);
                Pointer pointer = root?.GetComponent<Pointer>();
                pointer?.SetSelect();
                parent = parent.ParentPoint;
            }
            Transform endRoot = mapRoot.transform.Find(end.ToPoint());
            Pointer endPointer = endRoot?.GetComponent<Pointer>();
            endPointer?.SetSelect();
            coroutine = null;
        }));

    }

    private Point GetPoint(Point start, Point end)
    {
        return finder.FindPath(start, end, true);
    }

    private void OpenListCallBack(Point point)
    {
        string rootName = point.ToPoint();
        Transform root = mapRoot.transform.Find(rootName);
        Pointer pointer = root?.GetComponent<Pointer>();
        if (pointer)
        {
            pointer.SetSearch(point);
        }
    }
}
