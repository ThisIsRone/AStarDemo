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

    private void Awake()
    {
        map = new Map();
        finder = new Finder(map);
        mapRoot = transform.Find("Scroll View/Viewport/Content");
        modlePointer = transform.Find("Pointer").gameObject;
        grid = mapRoot.GetComponent<GridLayoutGroup>();
        sldWidth.onValueChanged.AddListener(sldWidthChange);
        sldHeight.onValueChanged.AddListener(sldHeighChange);
        btnBuildMap.onClick.AddListener(onClickDrawMap);
        btnFinder.onClick.AddListener(onClickFinder);
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
        int width = Mathf.FloorToInt(sldWidth.value);
        int height = Mathf.FloorToInt(sldHeight.value);
        map.RebuildMap(width, height);
        CreateMap(map);
    }

    private void onClickFinder()
    {
        var point = finder.FindPath();
        point.PrintPath();
    }

    private void Start()
    {
        onClickDrawMap();
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
