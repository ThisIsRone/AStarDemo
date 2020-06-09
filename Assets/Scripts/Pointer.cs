using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{

    private Map map;

    private int x,y;

    private Vector2 fator = new Vector2(80,80);

    private Image img = null;

    private void Awake()
    {
        img = transform.GetComponent<Image>();
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(Switch);
    }

    public void SetPointer(Map map,int x,int y)
    {
        this.map = map;
        this.x = x;
        this.y = y;
        transform.name = ToString();
        if (map.GetValue(x, y) > 0)
        {
            SetObstacle();
        }
        else
        {
            SetNormal();
        }
    }

    public void Switch()
    {
        if (map.GetValue(x,y) == 0)
        {
            map.SetValue(x, y, 1);
            SetObstacle();           
        }
        else
        {
            map.SetValue(x, y, 0);
            SetNormal();
        }
    }

    private void SetNormal()
    {
        img.color = Color.white;
    }

    private void SetObstacle()
    {
        img.color = Color.red;
    }

    public override string ToString()
    {
        return string.Format("(x:{0},y:{1})", x, y);
    }

}