using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    private Map map;

    private int x,y;

    private Vector2 fator = new Vector2(80,80);

    public void SetPointer(Map map,int x,int y)
    {
        this.map = map;
        this.x = x;
        this.y = y;
        transform.name = ToString();
        if (map.GetValue(x, y) > 0)
        {
            SetShow();
        }
        else
        {
            SetHide();
        }
    }

    public void Switch()
    {
        if (map.GetValue(x,y) == 0)
        {
            map.SetValue(x, y, 1);
            SetShow();           
        }
        else
        {
            map.SetValue(x, y, 0);
            SetHide();
        }
        Debug.LogError("切换状态:" + ToString());
    }

    private void SetShow()
    {
        transform.localPosition = new Vector3(-x * fator.x, y * fator.y);
        transform.localScale = Vector3.one;
    }

    private void SetHide()
    {
        transform.localPosition = new Vector3(-x * fator.x, y * fator.y);
        transform.localScale = Vector3.one;
    }

    public override string ToString()
    {
        return string.Format("(x:{0},y:{1})", x, y);
    }

}